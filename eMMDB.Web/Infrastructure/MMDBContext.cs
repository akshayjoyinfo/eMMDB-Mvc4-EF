using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using eMMDB.Domain;

namespace eMMDB.Web.Infrastructure
{
    public class MMDBContext:DbContext
    {
        public MMDBContext():base("MMDBConnectionString")
        {
            Database.SetInitializer<MMDBContext>(new CreateDatabaseIfNotExists<MMDBContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>().ToTable("Actors");
            modelBuilder.Entity<Producer>().ToTable("Producers");
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Producer> Producers { get; set; }
      
    }
}