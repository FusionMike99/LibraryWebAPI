using System.Collections.Generic;

namespace Business.Models
{
    public class ReaderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ICollection<int> CardsIds { get; set; } = new List<int>();
    }
}
