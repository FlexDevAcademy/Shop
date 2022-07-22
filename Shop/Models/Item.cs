using System.Collections.Generic;

namespace Shop.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ProfileId { get; set; }
        public bool IsOrdered { get; set; }
        public int? OrderId { get; set; }
        public List<ShoppingBag> ShoppingBags { get; set; }
    }
}
