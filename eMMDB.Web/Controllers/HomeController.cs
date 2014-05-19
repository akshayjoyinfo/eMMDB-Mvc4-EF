using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eMMDB.Web.Infrastructure;
using eMMDB.Domain;
using System.IO;
using System.Drawing;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.SqlClient;

namespace eMMDB.Web.Controllers
{
    public class HomeController : Controller
    {

        private MMDBContext _db = new MMDBContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Sample MVC4 -Entity FrameWork Code First Applciation";


            return View(_db.Movies.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Movie movieModel)
        {
            HttpPostedFileBase file = Request.Files[0];
            byte[] imageSize = null;
            if (file != null)
            {
                imageSize = new byte[file.ContentLength];
                file.InputStream.Read(imageSize, 0, (int)file.ContentLength);
                movieModel.Poster = imageSize;
            }
            List<int> listAcotrIDs = new List<int>();
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.StartsWith("MovieActors_Person_ID_"))
                {
                    int AcotrId= (Convert.ToInt32(Request.Form[key]));
                    Person person = _db.Persons.First(p => p.PersonID == AcotrId);
                    bool ifExists = _db.Actors.Any(shi=>shi.PersonID == AcotrId);
                    if (ifExists== false)
                    {
                        movieModel.MoviesActors.Add(_db.Actors.Add(new Actor() { PersonID = person.PersonID, Sex = person.Sex, Name=person.Name }));
                    }
                    else
                    {
                        movieModel.MoviesActors.Add(_db.Actors.Add(_db.Actors.First(p=>p.PersonID==AcotrId)));
                    }
                }
            }

            if (Request.Form["producerID"] != null)
            {
                string[] producerInfo = Request.Form["producerID"].ToString().Split(new string[] { "----" }, StringSplitOptions.None);
                int producerID = Convert.ToInt32(producerInfo[0]);
                Person person = _db.Persons.First(p => p.PersonID == producerID);
                bool prducerExist = _db.Producers.Any(shi => shi.PersonID == producerID);

                if (prducerExist == false)
                {
                    movieModel.MovieProducer = _db.Producers.Add(new Producer() { PersonID = producerID, Name=person.Name, Sex=person.Sex });
                }
                else
                {
                    movieModel.MovieProducer = _db.Producers.First(p=>p.PersonID==producerID);
                }

            }
            
            _db.Movies.Add(movieModel);
            try
            {
                _db.SaveChanges();
                TempData["Notification"] = "Successfully Created the Movie " + movieModel.Title;
            }
            catch (DbEntityValidationException e)
            {
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult PeopleSearch()
        {
            string q = Request.QueryString[0];
            //var productNames = (from p in _db.Persons where p.Name.Contains(q) select p).Distinct().Take(10).ToList();
            var productNames = (from p in _db.Persons where p.Name.Contains(q) select new { id = p.PersonID, p.Name }).Distinct().Take(10).ToList();
            //string content = string.Join<string>("\n", productNames);
            return Json(productNames, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPoster(int posterID)
        {
            byte[] photo = GetPosterFromDb(posterID);
            return File(photo, "image/jpeg");
        }

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private byte[] GetPosterFromDb(int posterID)
        {
            Movie mov = _db.Movies.First(m => m.MovieID == posterID);
            if (mov.Poster != null)
            {
                if (mov.Poster.LongLength != 0)
                {
                    return mov.Poster;
                }
                else
                {
                    var dir = Server.MapPath("/Images");
                    var path = Path.Combine(dir, "NoPoster" + ".png");
                    Image noPosterImg = Image.FromFile(path);
                    return imageToByteArray(noPosterImg);
                }
            }
            else
            {
                var dir = Server.MapPath("/Images");
                var path = Path.Combine(dir, "NoPoster" + ".png");
                Image noPosterImg = Image.FromFile(path);
                return imageToByteArray(noPosterImg);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int id = 0)
        {
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult Delete(int id = 0)
        {
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        //
        // POST: /Person/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = _db.Movies.Find(id);
            _db.Movies.Remove(movie);
            _db.SaveChanges();
            TempData["Notification"] = "Successfully Deleted the Movie " + movie.Title;
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id = 0)
        {
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movieModel)
        {
            
                
               // Movie movModel = _db.Movies.First(p => p.MovieID == movieModel.MovieID);
            Movie movModel = (from s in _db.Movies where s.MovieID == movieModel.MovieID select s).FirstOrDefault();

                if (Request.Files.Count != 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    byte[] imageSize = null;
                    if (file != null)
                    {
                        imageSize = new byte[file.ContentLength];
                        file.InputStream.Read(imageSize, 0, (int)file.ContentLength);
                        movModel.Poster = imageSize;
                    }
                   
                }
                else
                {
                    movModel.Poster = GetPosterFromDb(movModel.MovieID);
                }

                List<int> listActorIDsOriginal = movModel.MoviesActors.Select(p => p.PersonID).ToList();
                List<int> listAcotrIDsModified = new List<int>();
                            
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.StartsWith("MovieActors_Person_ID_"))
                    {
                        int AcotrId = (Convert.ToInt32(Request.Form[key]));
                        listAcotrIDsModified.Add(AcotrId);
                    }
                }

                IEnumerable<int> differenceQuery =
                  listActorIDsOriginal.Except(listAcotrIDsModified);

                IEnumerable<int> addQuery =
                 listAcotrIDsModified.Except(listActorIDsOriginal);

                foreach (int s in differenceQuery)
                {
                    Actor actor = _db.Actors.Single(p => p.PersonID == s);
                    movModel.MoviesActors.Remove(actor);
                    _db.SaveChanges();
                }

                foreach (int s in addQuery)
                {
                    Person person = _db.Persons.First(p => p.PersonID == s);
                    if (movModel.MoviesActors.Any(shi => shi.PersonID == s) == false)
                    {
                        bool ifExists = _db.Actors.Any(shi => shi.PersonID == s);
                        if (ifExists == false)
                        {
                            movModel.MoviesActors.Add(_db.Actors.Add(new Actor() { PersonID = person.PersonID, Sex = person.Sex, Name = person.Name }));
                        }
                        else
                        {
                            movModel.MoviesActors.Add(_db.Actors.Add(_db.Actors.First(p => p.PersonID == s)));
                        }
                    }
                }

                
                //_db.Movies.Attach(movieModel);
                movieModel.MoviesActors = movModel.MoviesActors;
                
                
                

                if (Request.Form["producerID"] != null)
                {
                    string[] producerInfo = Request.Form["producerID"].ToString().Split(new string[] { "----" }, StringSplitOptions.None);
                    int producerID = Convert.ToInt32(producerInfo[0]);
                   
                    Person person = _db.Persons.First(p => p.PersonID == producerID);
                    if (movModel.MovieProducer.PersonID!= producerID)
                    {

                        bool prducerExist = _db.Producers.Any(shi => shi.PersonID == producerID);

                        if (prducerExist == false)
                        {
                            movModel.MovieProducer = _db.Producers.Add(new Producer() { PersonID = producerID, Name = person.Name, Sex = person.Sex });
                        }
                        else
                        {
                            movModel.MovieProducer = _db.Producers.First(p => p.PersonID == producerID);
                        }
                    }
                    
                    
                    movieModel.MovieProducer = movModel.MovieProducer;
                }
                movieModel.Poster = movModel.Poster;
                //_db.Entry(movieModel).State = EntityState.Modified;

                string updateSQL = "Update [Movies] SET Title = @Title, ReleaseYear=@RelYear, Plot=@Plot  WHERE MovieID = @Id";
                _db.Database.ExecuteSqlCommand(updateSQL, new SqlParameter("@Title", movieModel.Title), 
                                                new SqlParameter("@RelYear", movieModel.ReleaseYear), 
                                                new SqlParameter("@Plot", movieModel.Plot), 
                                                new SqlParameter("@Id", movieModel.MovieID));
                
                _db.SaveChanges();
                TempData["Notification"] = "Successfully Edited the Movie " + movieModel.Title ;
                return RedirectToAction("Index");
            
        }


    }   
}
