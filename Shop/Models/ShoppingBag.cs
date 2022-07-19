using System.Collections.Generic;

namespace Shop.Models
{
    public class ShoppingBag
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public List<Item> Items { get; set; }
    }
}
