namespace RealEstate.Domain.Entities
{
    public class Owner
    {
        public Guid IdOwner { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Photo { get; private set; } = string.Empty;
        public DateTime Birthday { get; private set; }

        private Owner() { }

        public Owner(string name, string address, string photo, DateTime birthday)
        {
            IdOwner = Guid.NewGuid();
            Name = name;
            Address = address;
            Photo = photo;
            Birthday = birthday;
        }
    }
}