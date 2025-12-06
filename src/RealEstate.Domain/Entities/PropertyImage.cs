namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        public Guid IdPropertyImage { get; private set; }
        public Guid IdProperty { get; private set; }
        public string File { get; private set; } = string.Empty;
        public bool Enabled { get; private set; } = true;

        private PropertyImage() { }

        public PropertyImage(Guid idProperty, string file)
        {
            if (idProperty == Guid.Empty) throw new ArgumentException("Property id required", nameof(idProperty));
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("File required", nameof(file));

            IdPropertyImage = Guid.NewGuid();
            IdProperty = idProperty;
            File = file;
            Enabled = true;
        }

        public void Disable() => Enabled = false;
    }
}