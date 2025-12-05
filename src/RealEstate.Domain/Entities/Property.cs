namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }

        public Guid IdOwner { get; set; }
        public Owner? Owner { get; set; }

        private readonly List<PropertyImage> _images = new();
        public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

        private readonly List<PropertyTrace> _traces = new();
        public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

        private Property() { }

        public Property(string name, string address, decimal price, string codeInternal, int year, Guid idOwner)
        {
            IdProperty = Guid.NewGuid();
            Name = name;
            Address = address;
            Price = price;
            CodeInternal = codeInternal;
            Year = year;
            IdOwner = idOwner;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0) throw new ArgumentException("Price must be > 0");
            Price = newPrice;
        }

        public void AddImage(PropertyImage img) => _images.Add(img);
        public void AddTrace(PropertyTrace trace) => _traces.Add(trace);
    }
}