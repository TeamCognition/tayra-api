using System;
using System.Globalization;
using System.Linq;
using Cog.Core;
using Newtonsoft.Json;

namespace Tayra.Common
{
    public partial class TableData
    {
        protected Header[] Headers { get; }
        private object[] Records { get; }

        public TableData(object[] records)
        {
            this.Records = records;
                
            Headers = records.FirstOrDefault().GetType().GetProperties().Select(p => 
                    new Header((Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType).Name, p.Name))
                .ToArray();
        }

        protected class Header
        {
            private string Type { get; }
            private string Accessor { get; }

            public Header(string type, string accessor)
            {
                this.Type = type.ToLowerFirst();
                this.Accessor = accessor.ToLowerFirst();
            }
        }
    }

    public partial class TableData
    {
        [JsonConverter(typeof(ToStringJsonConverter))]
        public class Profile
        {
            private string Name { get; }
            private string Username { get; }

            public Profile(string name, string username)
            {
                this.Name = name;
                this.Username = username;
            }

            public override string ToString() => $"{Name}\0{Username}";
        }
    }

    public partial class TableData
    {
        [JsonConverter(typeof(ToStringJsonConverter))]
        public class ExternalLink
        {
            private string Text { get; }
            private string Url { get; }

            public ExternalLink(string text, string url)
            {
                this.Text = text;
                this.Url = url;
            }

            public override string ToString() => $"{Text}\0{Url}";
        }
    }
    
    public partial class TableData
    {
        [JsonConverter(typeof(ToStringJsonConverter))]
        public class TimeInMinutes
        {
            private int Minutes { get; }

            public TimeInMinutes(int? minutes)
            {
                this.Minutes = minutes ?? 0;
            }

            public override string ToString() => Minutes.ToString(CultureInfo.InvariantCulture);
        }
    }
}