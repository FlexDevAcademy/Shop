namespace Shop.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string StreetNumber { get; set; }
        public int ProfileId { get; set; }
    }
}
