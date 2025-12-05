namespace RealEstate.Contracts.DTOs
{
    public class PropertyImageDto
    {
        public string IdPropertyImage { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}