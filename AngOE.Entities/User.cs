using System;
using System.Collections.Generic;

namespace AngOE.Entities
{
    public class User:Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string AuthenticationToken { get; set; }
        public DateTime? AuthenticationTokenValidTo { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenValidTo { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}