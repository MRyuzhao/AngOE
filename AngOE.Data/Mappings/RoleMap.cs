using System.Data.Entity.ModelConfiguration;
using AngOE.Entities;

namespace AngOE.Data.Mappings
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            HasMany(x => x.Users)
                .WithMany(y => y.Roles)
                .Map(z =>
                {
                    z.MapLeftKey("UserId");
                    z.MapRightKey("RoleId");
                    z.ToTable("UserRoles");
                });
        }
    }
}