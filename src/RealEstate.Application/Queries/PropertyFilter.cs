namespace RealEstate.Application.Queries
{
    public record PropertyFilter(decimal? MinPrice = null, decimal? MaxPrice = null, Guid? OwnerId = null, int? Year = null);
}
