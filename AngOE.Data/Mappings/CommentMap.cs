using System.Data.Entity.ModelConfiguration;
using AngOE.Entities;

namespace AngOE.Data.Mappings
{
    public class CommentMap: EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            //Primary Key
            this.HasKey(x => x.Id);

            //Properties
            
            //Table & Column Mappings
            this.ToTable("Comment");
            this.Property(x => x.Id).HasColumnName("Id");
            this.Property(x => x.Body).HasColumnName("Body");
            this.Property(x => x.CreationTime).HasColumnName("CreationTime");

            // Relationships
            HasRequired(x => x.Blog)
                .WithMany(y => y.Comments)
                .HasForeignKey(z => z.BlogId);

            HasRequired(x => x.User)
                .WithMany(y => y.Comments)
                .HasForeignKey(z => z.PosterId);
        }
    }
}