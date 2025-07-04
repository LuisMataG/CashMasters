using CashMasters.ChangeCalculator.Interfaces;
using System.Text.Json;

namespace CashMasters.ChangeCalculator.Configuration
{
    /// <summary>
    /// Class responsible for managing the application configuration using a JSON file.
    /// Implements the IAppConfigManager interface.
    /// </summary>
    public class JsonConfigManager : IAppConfigManager
    {
        // Path where the configuration file will be saved or read from
        private const string ConfigFilePath = "appconfig.json";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Loads the configuration from the appconfig.json file.
        /// If the file does not exist, returns a new default instance of AppConfig.
        /// </summary>
        public AppConfig Load()
        {
            if (!File.Exists(ConfigFilePath))
                return new AppConfig();

            var json = File.ReadAllText(ConfigFilePath);

            // Deserialize the content into an AppConfig object, or return a new one if deserialization fails
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }

        /// <summary>
        /// Saves the current configuration to the appconfig.json file.
        /// </summary>
        public void Save(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, SerializerOptions);
            File.WriteAllText(ConfigFilePath, json);
        }
    }
}
