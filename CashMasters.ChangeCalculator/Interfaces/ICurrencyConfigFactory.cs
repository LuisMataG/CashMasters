namespace CashMasters.ChangeCalculator.Interfaces
{
    public interface ICurrencyConfigFactory
    {
        ICurrencyConfig Create(string option);
    }
}
