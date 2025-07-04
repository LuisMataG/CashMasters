using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CashMasters.ChangeCalculator.Interfaces;

namespace CashMasters.ChangeCalculator.Configuration
{
    public class CurrencyConfigFactory : ICurrencyConfigFactory
    {
        public ICurrencyConfig Create(string option)
        {
            List<decimal> denominations;

            switch (option.Trim().ToLower())
            {
                case "1":
                case "mxn":
                    denominations = CurrencyDefaults.MXN;
                    break;

                case "2":
                case "usd":
                    denominations = CurrencyDefaults.USD;
                    break;

                case "3":
                case "custom":
                    Console.WriteLine("Ingrese las denominaciones separadas por comas (e.g. 0.01,0.05,1,2):");
                    var customInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(customInput))
                        throw new ArgumentException("No denominations were entered.");
                    denominations =
                    [
                        .. customInput.Split(',')
                                                .Select(x => decimal.Parse(x.Trim(), CultureInfo.InvariantCulture))
                                                .OrderByDescending(x => x),
                    ];
                    break;

                default:
                    throw new ArgumentException("Invalid option");
            }

            return new GlobalCurrencyConfig(denominations);
        }
    }
}
