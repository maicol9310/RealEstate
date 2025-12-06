namespace RealEstate.Contracts.DTOs
{
    public class PropertyTraceDto
    {
        public Guid IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}