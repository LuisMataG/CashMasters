using System.Collections.Generic;
using CashMasters.ChangeCalculator.Core;

namespace CashMasters.ChangeCalculator.Interfaces
{
    public interface ICurrencyChangeCalculator
    {
        ChangeResult CalculateChange(decimal price, List<decimal> payment);
    }
}
