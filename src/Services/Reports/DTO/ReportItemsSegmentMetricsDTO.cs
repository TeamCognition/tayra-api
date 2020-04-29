namespace Tayra.Services
{
    public class ReportItemsSegmentMetricsDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public int ItemsCreated { get; set; }
        public int ItemsGifted { get; set; }
        public int ItemsDisenchanted { get; set; }

        public PurchasesDTO[] Purchases { get; set; }

        public class PurchasesDTO
        {
            public int Total { get; set; }
            public int Average { get; set; }
            //public ItemDTO[] Items { get; set; }

            public class ItemDTO
            {
                public string Name { get; set; }
                public float Price { get; set; }
            }
        }
    }
}
