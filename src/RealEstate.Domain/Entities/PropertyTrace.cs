namespace RealEstate.Domain.Entities
{
    public class PropertyTrace
    {
        public Guid IdPropertyTrace { get; private set; }
        public DateTime DateSale { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Value { get; private set; }
        public decimal Tax { get; private set; }
        public Guid IdProperty { get; private set; }
        public Property Property { get; private set; } = default!;

        private PropertyTrace() { } 

        public PropertyTrace(DateTime dateSale, string name, decimal value, decimal tax, Guid idProperty)
        {
            if (idProperty == Guid.Empty)
                throw new ArgumentException("Property id required", nameof(idProperty));

            IdPropertyTrace = Guid.NewGuid();
            DateSale = dateSale;
            Name = name;
            Value = value;
            Tax = tax;
            IdProperty = idProperty;
        }
    }
}