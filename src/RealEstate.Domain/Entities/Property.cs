namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public Guid IdProperty { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string CodeInternal { get; private set; } = string.Empty;
        public int Year { get; private set; }

        public Guid IdOwner { get; private set; }
        public Owner Owner { get; private set; } = default!;

        private readonly List<PropertyImage> _images = new();
        public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

        private readonly List<PropertyTrace> _traces = new();
        public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

        private Property() { } // EF

        public Property(string name, string address, decimal price, string codeInternal, int year, Guid idOwner)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than 0", nameof(price));

            if (idOwner == Guid.Empty)
                throw new ArgumentException("Owner required", nameof(idOwner));

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
            if (newPrice <= 0)
                throw new ArgumentException("Price must be > 0", nameof(newPrice));

            Price = newPrice;
        }

        public void Update(string name, string address, decimal price, string codeInternal, int year, Guid idOwner)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than 0", nameof(price));

            if (idOwner == Guid.Empty)
                throw new ArgumentException("Owner required", nameof(idOwner));

            Name = name;
            Address = address;
            Price = price;
            CodeInternal = codeInternal;
            Year = year;
            IdOwner = idOwner;
        }

        public void AddImage(PropertyImage img)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));
            _images.Add(img);
        }

        public void AddTrace(PropertyTrace trace)
        {
            if (trace == null) throw new ArgumentNullException(nameof(trace));
            _traces.Add(trace);
        }
    }
}