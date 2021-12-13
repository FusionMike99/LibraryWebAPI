using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Card : BaseEntity
    {
        public DateTime Created { get; set; }
        public int ReaderId { get; set; }

        public virtual Reader Reader { get; set; }
        public virtual ICollection<History> Books { get; set; } = new List<History>();
    }
}
