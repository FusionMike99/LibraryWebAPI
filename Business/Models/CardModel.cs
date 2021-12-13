using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int ReaderId { get; set; }
        public ICollection<int> BooksIds { get; set; } = new List<int>();
    }
}
