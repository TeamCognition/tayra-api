using System;

namespace Cog.Core
{
    public class Value
    {
        public Value(double? current, double? previous)
        {
            Current = current.HasValue && double.IsNaN(current.Value) ? null : current;
            Previous = previous.HasValue && double.IsNaN(previous.Value) ? null : previous;
        }

        public double? Current { get; set; }

        public double? Previous { get; set; }

        public double? Change
        {
            get
            {
                if (Previous == null || Current == null) return null;

                if (Previous.Value.Equals(0)) return 0;

                return Math.Round((Current.Value - Previous.Value) / Previous.Value * 100, 1);
            }
        }

        public static Value operator /(Value v1, Value v2)
        {
            double? current = null;
            double? previous = null;

            if (v1.Current.HasValue && v2.Current.HasValue && !v2.Current.Value.Equals(0))
                current = Math.Round(v1.Current.Value / v2.Current.Value, 1);

            if (v1.Previous.HasValue && v2.Previous.HasValue && !v2.Previous.Value.Equals(0))
                previous = Math.Round(v1.Previous.Value / v2.Previous.Value, 1);

            return new Value(current, previous);
        }

        public static Value operator *(Value v1, double v2)
        {
            return new(v1.Current * v2, v1.Previous * v2);
        }
    }
}