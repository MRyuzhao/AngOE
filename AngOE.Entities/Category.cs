using System.Collections.Generic;

namespace AngOE.Entities
{
    public class Category: Entity
    {
        public string CategoryName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}