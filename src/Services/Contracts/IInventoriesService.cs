using Cog.Core;

namespace Tayra.Services
{
    public interface IInventoriesService
    {
        InventoryItemViewDTO GetInventoryItemViewDTO(int inventoryItemId);
        GridData<InventoryItemGridDTO> GetInventoryItemViewGridDTO(InventoryItemGridParams gridParams);
        void Activate(int profileId, InventoryItemActivateToggleDTO dto);
        void Deactivate(int profileId, InventoryItemActivateToggleDTO dto);
        void Gift(int profileId, InventoryItemGiftDTO dto);
        void Disenchant(int profileId, InventoryItemDisenchantDTO dto);
        void Give(int profileId, InventoryGiveDTO dto);
    }
}
