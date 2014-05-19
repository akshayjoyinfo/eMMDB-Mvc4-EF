using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eMMDB.Domain
{
    public class Actor : Person
    {
        public virtual List<Movie> Movies { get; set; }

        public Actor()
        {
            this.Movies = new List<Movie>();
        }
    }
}
