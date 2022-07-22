using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
