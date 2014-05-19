using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using eMMDB.Domain;

namespace eMMDB.Web.Models.ViewModel
{
    public class MovieViewModel
    {
        [Key]
        [Display(Name="Movie ID")]
        public virtual int MovieID { get; set; }
        [Required(ErrorMessage="Title can not be blank")]
        public virtual string Title { get; set; }
        
        [Display(Name="Released Year")]
        [Required(ErrorMessage = "Released Year can not be blank")]
        [RegularExpression(@"^\d{4}$",ErrorMessage="Please enter Valid Year")]
        public virtual int ReleaseYear { get; set; }

        public virtual string Plot { get; set; }
        public virtual byte[] Poster { get; set; }

        public virtual List<int> MoviesActorIds { get; set; }

        public virtual int SelctedMovieProducer { get; set; }

        public Movie MapToMovie(List<Actor> actors, Producer produceOfMovie)
        {
            var movieResult = new Movie();

            movieResult.Title = this.Title;
            movieResult.ReleaseYear = this.ReleaseYear;
            movieResult.Plot = this.Plot;

            foreach (var actor in actors)
            {
                actor.Movies.Add(movieResult);
                movieResult.MoviesActors.Add(actor);

            }

            movieResult.MovieProducer = produceOfMovie;
            movieResult.MovieProducer.ProducedMovies.Add(movieResult);
            return movieResult;

        }





        public MovieViewModel()
        {
            this.MoviesActorIds = new List<int>();

        }
    }
}