using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eMMDB.Domain
{
    public class Producer : Person
    {
        public virtual List<Movie> ProducedMovies { get; set; }

        public Producer()
        {
            this.ProducedMovies = new List<Movie>();
        }
    }
}
