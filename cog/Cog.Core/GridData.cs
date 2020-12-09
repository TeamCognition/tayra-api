using System.Collections.Generic;

namespace Cog.Core
{
    public class GridData<T> where T : class
    {
        public int Total { get; set; }
        public List<T> Records { get; set; }
    }
}