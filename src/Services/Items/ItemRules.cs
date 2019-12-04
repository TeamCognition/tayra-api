using Tayra.Common;

namespace Tayra.Services
{
    public static class ItemRules
    {
        public static bool IsItemTypeTayra(ItemTypes itemType)
        {
            return (int)itemType >= 100 && (int)itemType < 200;
        }
    }
}
