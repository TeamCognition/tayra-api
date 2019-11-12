using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class InventoryRules
    {
        public static bool CanActivateInventoryItem(int profileId, ProfileInventoryItem inventoryItem, Item item)
        {
            return profileId == inventoryItem.ProfileId
                && !inventoryItem.IsActive
                && item.IsActivable;
        }

        public static bool CanGiftInventoryItem(int senderId, int receiverId, int inventoryItemOwnerId, bool isGiftable)
        {
            return senderId == inventoryItemOwnerId
                && senderId != receiverId
                && isGiftable;
        }
    }
}
