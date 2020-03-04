﻿using System;
namespace Tayra.Services
{
    public class ShopViewDTO
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public DateTime Created { get; set; }
        public ShopStatisticDTO[] shopStatistics {get; set;}
        public class ShopStatisticDTO 
        {
            public int lastMonth;
            public int total;
        }
    }
}
