using System.Collections.Generic;

namespace CashMasters.ChangeCalculator.Interfaces
{
    public interface ICurrencyConfig
    {
        List<decimal> GetDenominations();
    }
}
