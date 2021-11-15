using System;
using System.Globalization;
using System.Linq;

namespace Cog.Core
{
    public class GridParams
    {
        public int Rows { get; set; }
        public int Page { get; set; }
        public string Sidx { get; set; }
        public string Sord { get; set; } = "ASC";
        public Filter[] Filters { get; set; }

        public bool IsValidPropertyName<T>() where T : class
        {
            var props = typeof(T).GetProperties().Select(p => p.Name.ToLower());

            return props.Any(p => Sidx.ToLower() == p) &&
                   (Filters == null
                    || Filters.All(x => props.Contains(x.Idx.ToLower()))
                    && Filters.Where(x => x.Type == Filter.FilterTypes.Match)
                        .All(x => x.Values.All(v =>
                            IsNumeric(v) || IsBoolean(v))) //maybe determine prop type by name and check with that?
                    && Filters.Where(x => x.Type == Filter.FilterTypes.Range)
                        .All(x => x.Values.All(v => IsNumeric(v) || IsDate(v)))
                    && Filters.Where(x => x.Type == Filter.FilterTypes.Like)
                        .All(x => x.Values.All(v => v.ToString().All(char.IsLetterOrDigit))));
        }

        public string Ordering()
        {
            Sord = Sord.ToUpper();
            if (Sord == "ASC" || Sord == "DESC") return Sidx + " " + Sord;
            return Sidx;
        }

        private static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            return double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture)
                , NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
        }

        private static bool IsBoolean(object expression)
        {
            if (expression == null)
                return false;

            return bool.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out _);
        }

        private static bool IsDate(object expression)
        {
            if (expression == null)
                return false;

            return DateTime.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out _);
        }

        public class Filter
        {
            public enum FilterTypes
            {
                Match = 1,
                Range = 2,
                Like = 3
            }

            public FilterTypes Type { get; set; }
            public string Idx { get; set; }
            public object[] Values { get; set; }
        }
    }
}