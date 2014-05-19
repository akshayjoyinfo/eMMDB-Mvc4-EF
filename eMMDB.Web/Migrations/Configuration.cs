namespace eMMDB.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using eMMDB.Domain;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<eMMDB.Web.Infrastructure.MMDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(eMMDB.Web.Infrastructure.MMDBContext context)
        {
            this.GetActors().ForEach(a => context.Actors.Add(a));
            this.GetProducers().ForEach(d => context.Producers.Add(d));

        }

        private List<Movie> GetMovies()
        {
            throw new NotImplementedException();
        }

        private List<Producer> GetProducers()
        {
            List<Producer> producers = new List<Producer>();

            producers.Add(new Producer() { Name = "Christopher Nolan", Dob = new DateTime(1970, 7, 11), Sex = "Male" });
            producers.Add(new Producer() { Name = "Steven Spielberg", Dob = new DateTime(1946, 12, 18), Sex = "Male" });
            producers.Add(new Producer() { Name = "Mel Gibson", Dob = new DateTime(1950, 4, 4), Sex = "Male" });
            producers.Add(new Producer() { Name = "Stanley Kubrick", Dob = new DateTime(1928, 6, 7), Sex = "Male" });
            producers.Add(new Producer() { Name = "Christian Bale", Dob = new DateTime(1974, 1, 30), Sex = "Male" });

            return producers;
        }

        private List<Actor> GetActors()
        {
            List<Actor> actors = new List<Actor>();

            actors.Add(new Actor() { Name = "Leonardo DiCaprio", Dob = new DateTime(1974,11,11), Sex="Male"});
            actors.Add(new Actor() { Name = "Morgan Freeman", Dob = new DateTime(1937,6,1), Sex="Male"});
            actors.Add(new Actor() { Name = "Robert Drowney Jr.", Dob = new DateTime(1965, 4, 4), Sex = "Male" });
            actors.Add(new Actor() { Name = "Johnny Depp", Dob = new DateTime(1963, 6, 7), Sex = "Male" });
            actors.Add(new Actor() { Name = "Christian Bale", Dob = new DateTime(1974, 1, 30), Sex = "Male" });
            actors.Add(new Actor() { Name = "Scarlett Johansson", Dob = new DateTime(1984, 11, 22), Sex = "Female" });
            actors.Add(new Actor() { Name = "Anne Hathaway", Dob = new DateTime(1982, 11, 12), Sex = "Female" });

            return actors;
        }
    }
}
