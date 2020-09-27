using System;
using System.Linq;

namespace Tayra.Common
{
    public class TableData<T> where T: class
    {
        public Header[] Headers { get; set; }
        public T[] Records { get; set; }

        public TableData(T[] records)
        {
            this.Records = records;
                
            Headers = records.FirstOrDefault().GetType().GetProperties().Select(p => 
                    new Header((Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType).Name, p.Name))
                .ToArray();
        }

        public class Header
        {
            public string Type { get; set; }
            public string Accessor { get; set; }

            public Header(string type, string accessor)
            {
                this.Type = type;
                this.Accessor = accessor;
            }
        }
    }
        
    public class TableDataProfile
    {
        public string Name { get; set; }
        public string Username { get; set; }

        public TableDataProfile(string name, string username)
        {
            this.Name = name;
            this.Username = username;
        }

        public override string ToString() => $"{Name}\0{Username}";
    }
        
    public class TableDataHyperLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}