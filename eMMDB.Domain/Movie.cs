using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace eMMDB.Domain
{
    public class Movie
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

        public virtual List<Actor> MoviesActors { get; set; }

        public virtual Producer MovieProducer { get; set; }

        

        public Movie()
        {
            this.MoviesActors = new List<Actor>();
           
        }
    }
}
