using System;
using Cog.Core;

namespace Tayra.Services
{
    public interface IInventoriesService
    {
        InventoryItemViewDTO GetInventoryItemViewDTO(Guid inventoryItemId);
        GridData<InventoryItemGridDTO> GetInventoryItemViewGridDTO(InventoryItemGridParams gridParams);
        void Activate(Guid profileId, InventoryItemActivateToggleDTO dto);
        void Deactivate(Guid profileId, InventoryItemActivateToggleDTO dto);
        void Gift(Guid profileId, InventoryItemGiftDTO dto);
        void Disenchant(Guid profileId, InventoryItemDisenchantDTO dto);
        void Give(Guid profileId, InventoryGiveDTO dto);
    }
}
