using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Tests.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Money_WhenAmountIsNegative_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => new Money(-100m, "LKR"));
        }

        [Fact]
        public void Money_ShouldNormalizeCurrencyToUpperCase()
        {
            var money = new Money(100m, "lkr");

            Assert.Equal("LKR", money.Currency);
        }
    }
}
