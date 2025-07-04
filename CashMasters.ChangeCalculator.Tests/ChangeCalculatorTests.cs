using CashMasters.ChangeCalculator.Core;
using CashMasters.ChangeCalculator.Configuration;

namespace CashMasters.ChangeCalculator.Tests
{
    public class ChangeCalculatorTests
    {
        // We create a data array for the tests
        public static IEnumerable<object[]> TestCases =>
            new List<object[]>
            {
                // Case 1: MXN-like config
                new object[]
                {
                    new List<decimal> { 0.05m, 0.10m, 0.20m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m, 200m, 500m, 1000m },
                    148.95m,
                    new List<decimal> { 100m, 20m, 20m, 10m },
                    new Dictionary<decimal, int> { { 1m, 1 }, { 0.05m, 1 } }
                },
                new object[]
                {
                    new List<decimal> { 0.05m, 0.10m, 0.20m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m, 200m, 500m, 1000m },
                    1055.90m,
                    new List<decimal> { 500m, 200m, 200m, 200m },
                    new Dictionary<decimal, int> { { 20m, 2 }, { 2m, 2 }, { 0.10m, 1 } }
                },

                // Case 2: USA-like config
                new object[]
                {
                    new List<decimal> { 0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m },
                    320.15,
                    new List<decimal> { 100m, 100m, 50m, 50m, 20m, 10m },
                    new Dictionary<decimal, int> { { 5m, 1 }, { 2m, 2 }, { 0.50m, 1 }, { 0.25m, 1 }, { 0.1m, 1 } }
                },
                new object[]
                {
                    new List<decimal> { 0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m },
                    99.75m,
                    new List<decimal> { 100m },
                    new Dictionary<decimal, int> { { .25m, 1 } }
                },
                
                // Case 3: custom-like config
                new object[]
                {
                    new List<decimal> { 1m, 5m, 10m, 20m },
                    19,
                    new List<decimal> { 20m },
                    new Dictionary<decimal, int> { { 1m, 1 } }
                },
                new object[]
                {
                    new List<decimal> { 0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1m },
                    1.45m,
                    new List<decimal> { 1m, 1m },
                    new Dictionary<decimal, int> { { .50m, 1 }, { .05m, 1 } }
                },

                // Case 4: errores
                new object[]
                {
                    new List<decimal> { 1m, 5m, 10m, 20m },
                    19.5,
                    new List<decimal> { 20m },
                    null
                },
                new object[]
                {
                    new List<decimal> { 1m },
                    1.45m,
                    new List<decimal> { 1m, 1m },
                    null
                },
                new object[]
                {
                    new List<decimal> { 1m, 5m, 10m, 20m },
                    25,
                    new List<decimal>(),
                    null
                },
                new object[]
                {
                    new List<decimal> { 1m, 5m, 10m, 20m },
                    25,
                    new List<decimal> { 20m },
                    null
                },
                new object[]
                {
                    new List<decimal> { 1m, 5m, 10m, 20m },
                    -5,
                    new List<decimal> { 20m },
                    null
                }
            };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void CalculateChange_ReturnsExpectedResults(
                    List<decimal> denominations,
                    decimal price,
                    List<decimal> payment,
                    Dictionary<decimal, int>? expectedChange)
        {
            // We create the configurations and instantiate the calculator.
            var config = new GlobalCurrencyConfig(denominations);
            var calculator = new CurrencyChangeCalculator(config);

            // If expectedChange is null, it means we were expecting an error.
            if (expectedChange == null)
            {
                // We validate whether the price or payment is invalid and catch an ArgumentException.
                if (price <= 0 || payment == null || payment.Count == 0)
                {
                    Assert.Throws<ArgumentException>(() =>
                        calculator.CalculateChange(price, payment));
                }
                else
                {
                    // An InvalidOperationException is caught when exact change cannot be returned.
                    Assert.Throws<InvalidOperationException>(() =>
                        calculator.CalculateChange(price, payment));
                }
                
            }
            else
            {
                // We start the call to the method under test
                var result = calculator.CalculateChange(price, payment);

                // We validate that the number of denominations in the test data matches the ones returned during execution
                Assert.Equal(expectedChange.Count, result.DenominationCounts.Count);

                // We iterate through the change dictionary to first validate that the returned result matches the test data
                foreach (var expected in expectedChange)
                {
                    Assert.True(result.DenominationCounts.TryGetValue(expected.Key, out int actualCount), $"Expected denomination {expected.Key} not found.");
                    Assert.Equal(expected.Value, actualCount);
                }
            }
        }
    }
}
