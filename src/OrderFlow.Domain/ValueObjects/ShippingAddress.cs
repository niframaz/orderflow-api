namespace OrderFlow.Domain.ValueObjects
{
    public sealed record ShippingAddress(string Street, string City, string PostalCode, string Country);
}
