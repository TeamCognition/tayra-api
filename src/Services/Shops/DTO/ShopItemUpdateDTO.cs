namespace Tayra.Services
{
    public class ShopItemUpdateDTO : ShopItemCreateDTO
    {
        public int ItemId { get; set; }
        public bool AffectOwnedItems { get; set; }
    }
}
