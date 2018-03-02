using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AngOE.Data.Mappings;
using AngOE.Data.Migrations;
using AngOE.Entities;

namespace AngOE.Data
{
    public class AngOeDbContext: DbContext
    {
        public AngOeDbContext() : base("name = AngOeDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AngOeDbContext,
                Configuration>(true));
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
