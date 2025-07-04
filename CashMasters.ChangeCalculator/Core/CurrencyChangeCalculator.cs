using System;
using System.Collections.Generic;
using System.Linq;
using CashMasters.ChangeCalculator.Interfaces;
using CashMasters.ChangeCalculator.Core;

namespace CashMasters.ChangeCalculator.Core
{
    public class CurrencyChangeCalculator(ICurrencyConfig currencyConfig) : ICurrencyChangeCalculator
    {
        private readonly ICurrencyConfig _currencyConfig = currencyConfig ?? throw new ArgumentNullException(nameof(currencyConfig));

        public ChangeResult CalculateChange(decimal price, List<decimal> payment)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            if (payment == null || payment.Count == 0)
                throw new ArgumentException("Payment must include one or more valid denominations.");

            decimal totalPaid = payment.Sum();
            if (totalPaid < price)
                throw new InvalidOperationException($"Insufficient payment. Paid: {totalPaid:C}, Price: {price:C}");

            decimal change = Math.Round(totalPaid - price, 2);
            var result = new ChangeResult();

            foreach (var denomination in _currencyConfig.GetDenominations())
            {
                int count = (int)(change / denomination);
                if (count > 0)
                {
                    result.DenominationCounts[denomination] = count;
                    change = Math.Round(change - (count * denomination), 2);
                }

                if (change == 0)
                    break;
            }

            if (change > 0)
                throw new InvalidOperationException($"Cannot return exact change. Remaining: {change:C}");

            return result;
        }
    }
}
