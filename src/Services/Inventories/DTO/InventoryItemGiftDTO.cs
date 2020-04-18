using System;

namespace Tayra.Services
{
    public class InventoryItemGiftDTO
    {
        public int InventoryItemId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime? DemoDate { get; set; }
    }
}
