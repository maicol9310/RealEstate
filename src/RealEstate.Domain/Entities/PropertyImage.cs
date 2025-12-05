namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        public Guid IdPropertyImage { get; set; }
        public Guid IdProperty { get; set; }
        public string File { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;

        private PropertyImage() { }

        public PropertyImage(Guid idProperty, string file)
        {
            IdPropertyImage = Guid.NewGuid();
            IdProperty = idProperty;
            File = file;
            Enabled = true;
        }
    }
}