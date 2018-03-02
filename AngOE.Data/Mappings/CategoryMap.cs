using System.Data.Entity.ModelConfiguration;
using AngOE.Entities;

namespace AngOE.Data.Mappings
{
    public class CategoryMap: EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            //Primary Key
            this.HasKey(x => x.Id);

            //Properties
            this.Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");
        }
    }
}