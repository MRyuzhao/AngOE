using System.Data.Entity.Migrations;
using System.Linq;
using AngOE.Entities;

namespace AngOE.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AngOeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(AngOeDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (!context.Users.Any(x => x.Name == "Admin"))
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "test@account.com",
                    PasswordHash = "$2a$05$ePyLEbvIOKr3AAEieZVapu0c9yG58hk6VCQC6KenGo3zy42Ur2EA2",
                });
            }
        }
    }
}
