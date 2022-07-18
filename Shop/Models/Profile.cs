namespace Shop.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Avatar { get; set; }
        public decimal Balance { get; set; }
        public Address Address { get; set; }
    }
}
