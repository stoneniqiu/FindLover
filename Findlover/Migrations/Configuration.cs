using System.Web.Providers.Entities;

namespace Findlover.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration<TContext> : DbMigrationsConfiguration<TContext> where TContext : DbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;    
        }

        protected override void Seed(TContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.Users.AddOrUpdate(
            //   p=>new Models.User
            //       {
            //           UserName = "stoneniqiu",
            //           Password = "123456",
            //           Email = "stonzrj@163.com",
            //           CreatTime = DateTime.Now,
            //       }
             
            //);
            
        }
    }
}
