namespace RealEstate.Domain.Entities
{
    public class PropertyTrace
    {
        public Guid IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public Guid IdProperty { get; set; }

        private PropertyTrace() { }

        public PropertyTrace(DateTime dateSale, string name, decimal value, decimal tax, Guid idProperty)
        {
            IdPropertyTrace = Guid.NewGuid();
            DateSale = dateSale;
            Name = name;
            Value = value;
            Tax = tax;
            IdProperty = idProperty;
        }
    }
}