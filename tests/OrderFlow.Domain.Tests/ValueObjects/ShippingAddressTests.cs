using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Tests.ValueObjects
{
    public class ShippingAddressTests
    {
        [Fact]
        public void ShippingAddress_ShouldContainRequiredValues()
        {
            var address = new ShippingAddress("123 Main Street", "Colombo", "00100", "Sri Lanka");

            Assert.Equal("123 Main Street", address.Street);
            Assert.Equal("Colombo", address.City);
            Assert.Equal("00100", address.PostalCode);
            Assert.Equal("Sri Lanka", address.Country);
        }
    }
}
