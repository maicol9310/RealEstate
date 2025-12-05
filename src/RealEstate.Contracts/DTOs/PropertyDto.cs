namespace RealEstate.Contracts.DTOs
{
    public class PropertyDto
    {
        public string IdProperty { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public OwnerDto Owner { get; set; } = new();
        public string? ImageFile { get; set; }
        public IEnumerable<PropertyTraceDto>? Traces { get; set; }
    }
}