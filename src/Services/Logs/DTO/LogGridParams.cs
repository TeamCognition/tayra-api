﻿using Firdaws.Core;

namespace Tayra.Services
{
    public class LogGridParams : GridParams
    {
        public string ProfileUsername { get; set; }
        public int? ProfileId { get; set; }
        public int? CompetitionId { get; set; }
        public int? TeamId { get; set; }
        public bool? ShopLogs { get; set; }
    }
}
