using System;
using System.Collections.Generic;
using System.Linq;
using CashMasters.ChangeCalculator.Interfaces;

namespace CashMasters.ChangeCalculator.Configuration
{
    public class GlobalCurrencyConfig : ICurrencyConfig
    {
        private readonly List<decimal> _denominations;

        public GlobalCurrencyConfig(List<decimal> denominations)
        {
            if (denominations == null || denominations.Count == 0)
                throw new ArgumentException("The denomination needs to be set up.");

            _denominations = [.. denominations.OrderByDescending(d => d)];
        }

        public List<decimal> GetDenominations() => _denominations;
    }
}
