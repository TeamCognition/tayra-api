using System;

namespace Tayra.Services
{
    public class FilterRowBodyDTO
    {
        public EntityRow[] Rows { get; set; }

        public class EntityRow
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
        }
    }
}