namespace RealEstate.Contracts.DTOs
{
    public class PropertyImageDto
    {
        public Guid IdPropertyImage { get; set; }
        public string File { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}