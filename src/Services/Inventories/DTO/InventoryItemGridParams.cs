using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class InventoryItemGridParams : GridParams
    {
        public string ProfileUsername { get; set; }
        public string ItemNameQuery { get; set; }

        public List<ItemTypes> ItemTypesQuery { get; set; }
    }
}
