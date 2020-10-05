using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cog.Core;
using Newtonsoft.Json;

namespace Tayra.Common
{
    public partial class TableData
    {
        public Column[] Columns { get; }
        
        public object[] Records { get; }
        
        public TableData(Type dataType, object[] records)
        {
            this.Records = records;
            Columns = dataType.GetProperties().Select(p => new Column(p)).ToArray();
        }

        public class Column
        {
            public string Type { get; }
            public string Accessor { get; }

            internal Column(PropertyInfo p)
            {
                this.Type = (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType).Name.ToLowerFirst();
                this.Accessor =  p.Name.ToLowerFirst();
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