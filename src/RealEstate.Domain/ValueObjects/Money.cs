namespace RealEstate.Domain.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; }
        public Money(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must be >= 0", nameof(amount));
            Amount = amount;
        }

        public Money Change(decimal newAmount) => new Money(newAmount);

        public override string ToString() => Amount.ToString("F2");
    }
}