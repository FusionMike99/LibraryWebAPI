using System.Collections.Generic;

namespace Data.Entities
{
    public class Reader : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ReaderProfile ReaderProfile { get; set; }
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
