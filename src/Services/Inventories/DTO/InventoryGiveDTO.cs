namespace Tayra.Services
{
    public class InventoryGiveDTO
    {
        public int ItemId { get; set; }
        public string ReceiverUsername { get; set; }
        //For demo
        public bool ClaimRequired { get; set; } = true;
    }
}
