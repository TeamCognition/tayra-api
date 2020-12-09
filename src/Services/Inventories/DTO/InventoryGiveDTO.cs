using System;

namespace Tayra.Services
{
    public class InventoryGiveDTO
    {
        public Guid ItemId { get; set; }
        public string ReceiverUsername { get; set; }
        //For demo
        public bool ClaimRequired { get; set; } = true;
    }
}
