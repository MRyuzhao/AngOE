using System;
using System.Collections.Generic;

namespace AngOE.Entities
{
    public class Blog: Entity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}