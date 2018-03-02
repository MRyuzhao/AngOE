using System.Data.Entity.ModelConfiguration;
using AngOE.Entities;

namespace AngOE.Data.Mappings
{
    public class BlogMap: EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            //Primary Key
            this.HasKey(x => x.Id);

            //Properties
            this.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(x => x.Body)
                .IsRequired();

            //Table & Column Mappings
            this.ToTable("Blog");
            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Body).HasColumnName("Body");
            this.Property(x => x.Title).HasColumnName("Title");
            this.Property(x => x.CreationTime).HasColumnName("CreationTime");
            this.Property(x => x.UpdateTime).HasColumnName("UpdateTime");

            // Relationships
            HasRequired(x => x.User)
                .WithMany(y => y.Blogs)
                .HasForeignKey(z => z.AuthorId);

            HasRequired(x => x.Category)
                .WithMany(y => y.Blogs)
                .HasForeignKey(z => z.CategoryId);
            
           
        }
    }
}