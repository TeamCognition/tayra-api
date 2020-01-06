using Tayra.Common;

namespace Tayra.Services
{
    public static class ItemRules
    {
        public static bool IsItemTypeTayra(ItemTypes itemType)
        {
            return (int)itemType >= 100 && (int)itemType < 200;
        }

        public static bool CanReserveQuantity(int? available, int toReserve)
        {
            return !available.HasValue || available.Value >= toReserve;
        }
    }
}
