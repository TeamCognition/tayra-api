using System;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class InventoryRules
    {
        public static bool CanActivateInventoryItem(Guid profileId, ProfileInventoryItem inventoryItem, Item item)
        {
            return profileId == inventoryItem.ProfileId
                && !inventoryItem.IsActive
                && item.IsActivable;
        }

        public static bool CanGiftInventoryItem(Guid senderId, Guid receiverId, Guid inventoryItemOwnerId, bool isGiftable, bool isActive)
        {
            return senderId == inventoryItemOwnerId
                && senderId != receiverId
                && isGiftable
                && !isActive;
        }
    }
}
