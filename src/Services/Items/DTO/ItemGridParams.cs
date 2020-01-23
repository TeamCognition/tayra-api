using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class ItemGridParams : GridParams
    {
        public List<ItemTypes> ItemTypesQuery { get; set; }

        /// <summary>
        /// null for both enabled and disabled
        /// </summary>
        public bool? OnlyUnlimitedQuantity { get; set; }
    }
}
