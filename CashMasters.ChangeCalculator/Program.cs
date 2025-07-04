using System;
using System.Collections.Generic;
using System.Globalization;
using CashMasters.ChangeCalculator.Core;
using CashMasters.ChangeCalculator.Interfaces;
using CashMasters.ChangeCalculator.Configuration;

namespace CashMasters.ChangeCalculator
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Load saved configuration
                IAppConfigManager configManager = new JsonConfigManager();
                AppConfig appConfig = configManager.Load();

                // Configure the currency, allowing the user to keep or change the saved one
                string option = GetCurrencyConfiguration(appConfig, configManager);
                Console.Clear();

                // Create the currency denomination configuration based on the selected option
                ICurrencyConfigFactory factory = new CurrencyConfigFactory();
                ICurrencyConfig currencyConfig = factory.Create(option);

                // Create the change calculator with the currency configuration
                ICurrencyChangeCalculator calculator = new CurrencyChangeCalculator(currencyConfig);

                StartCalculator(calculator, currencyConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nConfiguration error: {ex.Message}");
            }
        }

        static string GetCurrencyConfiguration(AppConfig appConfig, IAppConfigManager configManager)
        {
            string? option = appConfig.SelectedCurrencyOption;

            // Give the user the option to modify the configuration if one was previously saved
            if (!string.IsNullOrWhiteSpace(option))
            {
                Console.WriteLine($"Previously selected currency: {option}");
                Console.WriteLine("Do you want to change the configuration?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("Any key to start calculator");
                Console.Write("Select an option: ");

                string? seleccion = Console.ReadLine();
                if (seleccion != "1")
                {
                    // Return the saved configuration
                    return option;
                }
            }

            // Return the new configuration
            return SetCurrencyConfiguration(appConfig, configManager);
        }

        static string SetCurrencyConfiguration(AppConfig appConfig, IAppConfigManager configManager)
        {
            Console.Clear();
            Console.WriteLine("Select the currency type:");
            Console.WriteLine("1. MXN (Mexican Pesos)");
            Console.WriteLine("2. USD (US Dollar)");
            Console.WriteLine("3. Custom");

            string? option;
            int numericOption;

            // Allow the user to select the configuration; validate that it's a number between 1 and 3
            do
            {
                Console.Write("Option: ");
                option = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(option) ||
                    !int.TryParse(option, out numericOption) ||
                    numericOption < 1 || numericOption > 3)
                {
                    Console.WriteLine("Invalid selection. Please try again..\n");
                }
            } while (string.IsNullOrWhiteSpace(option) ||
                     !int.TryParse(option, out numericOption) ||
                     numericOption < 1 || numericOption > 3);

            // Save the configuration only when it is one of the predefined options (USD, MXN)
            if (numericOption != 3)
            {
                appConfig.SelectedCurrencyOption = option;
                configManager.Save(appConfig);
            }

            Console.Clear();
            return option;
        }

        static void StartCalculator(ICurrencyChangeCalculator calculator, ICurrencyConfig currencyConfig)
        {
            while (true)
            {
                Console.WriteLine("\n---------------------------------------------");
                Console.Write("Enter item price (or type 'exit' to quit): ");
                var priceInput = Console.ReadLine()?.Trim();

                // Ends the application when the user types 'exit'
                if (priceInput?.ToLower() == "exit")
                    Environment.Exit(0);

                // Validate that the price is a number greater than 0
                if (!decimal.TryParse(priceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) || price <= 0)
                {
                    Console.WriteLine("Invalid price. Please enter a number greater than 0.");
                    continue;
                }

                // Get the denominations used to make the payment
                List<decimal> payment = GetPaid(price, currencyConfig);

                try
                {
                    // Calculate the change
                    var result = calculator.CalculateChange(price, payment);
                    Console.WriteLine("\nChange to return:");
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calculating change: {ex.Message}");
                }
            }
        }

        static List<decimal> GetPaid(decimal price, ICurrencyConfig currencyConfig)
        {
            var payment = new List<decimal>();
            decimal totalPaid = 0;

            // Ask the user how many coins or bills were received for each denomination until the price is covered
            Console.WriteLine("Enter how many coins and bills of each denomination were received:");

            foreach (var denom in currencyConfig.GetDenominations())
            {
                if (totalPaid >= price) break;

                Console.Write($"How many of {denom:C}?: ");
                var countInput = Console.ReadLine();

                if (int.TryParse(countInput, out int count) && count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        payment.Add(denom);
                        totalPaid += denom;
                        if (totalPaid >= price) break;
                    }
                }
            }

            return payment;
        }
    }
}