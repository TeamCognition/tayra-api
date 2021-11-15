using System;
namespace Tayra.Services
{
    public class ShopViewDTO
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public int TotalRequests { get; set; }
        public DateTime Created { get; set; }
        public ShopStatisticDTO ItemStats { get; set; }
        public ShopStatisticDTO TokenStats { get; set; }

        public class ShopStatisticDTO
        {
            public float Last30 { get; set; }
            public float Total { get; set; }
        }
    }
}
