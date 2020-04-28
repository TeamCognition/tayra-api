using System;

namespace Tayra.Services
{
    public class InventoryItemGiftDTO
    {
        public int InventoryItemId { get; set; }
        public int ReceiverId { get; set; }
        public bool ClaimRequired { get; set; } = true;
        public DateTime? DemoDate { get; set; }
    }
}
