using CashMasters.ChangeCalculator.Configuration;

namespace CashMasters.ChangeCalculator.Interfaces
{
    public interface IAppConfigManager
    {
        AppConfig Load();
        void Save(AppConfig config);
    }
}
