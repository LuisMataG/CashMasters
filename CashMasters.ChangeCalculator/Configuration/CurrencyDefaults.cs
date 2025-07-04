using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMasters.ChangeCalculator.Configuration
{
    public static class CurrencyDefaults
    {
        public static readonly List<decimal> MXN =
        [
            0.05m, 0.10m, 0.20m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m, 200m, 500m, 1000m
        ];

        public static readonly List<decimal> USD =
        [
            0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1m, 2m, 5m, 10m, 20m, 50m, 100m
        ];
    }
}
