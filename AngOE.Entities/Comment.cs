using System;

namespace AngOE.Entities
{
    public class Comment:Entity
    {
        public string Body { get; set; }

        public DateTime CreationTime { get; set; }

        public int BlogId { get; set; }
        public int PosterId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}