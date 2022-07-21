using System.Collections.Generic;

namespace Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal OrderValue { get; set; }
        public List<Item> Items { get; set; }
    }
}
