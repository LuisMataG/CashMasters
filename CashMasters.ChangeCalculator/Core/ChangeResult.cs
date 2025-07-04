using System;
using System.Collections.Generic;
using System.Linq;

namespace CashMasters.ChangeCalculator.Core
{
    public class ChangeResult
    {
        public Dictionary<decimal, int> DenominationCounts { get; set; } = [];

        public override string ToString()
        {
            if (DenominationCounts.Count == 0) return "No change needed.";
            return string.Join("\n", DenominationCounts.Select(kv => $"{kv.Value} x {kv.Key:C}"));
        }
    }
}
