namespace RealEstate.Contracts.DTOs
{
    public class PropertyDto
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public OwnerDto? Owner { get; set; }
        public string? ImageFile { get; set; }
        public IEnumerable<PropertyTraceDto>? Traces { get; set; }
        public IEnumerable<PropertyImageDto>? Images { get; set; }
    }
}