using System.Data.Entity.ModelConfiguration;
using AngOE.Entities;

namespace AngOE.Data.Mappings
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PasswordHash)
                .HasMaxLength(128);

            this.Property(t => t.AuthenticationToken)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.AuthenticationToken).HasColumnName("AuthenticationToken");
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.AuthenticationTokenValidTo).HasColumnName("AuthenticationTokenValidTo");
            this.Property(t => t.ResetPasswordTokenValidTo).HasColumnName("ResetPasswordTokenValidTo");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");

        }
    }
}