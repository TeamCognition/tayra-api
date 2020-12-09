using System;
using Cog.Core;

namespace Tayra.Services
{
    public class ProfileGridParams : GridParams
    {
        public Guid? segmentIdExclude { get; set; }
        public string UsernameQuery { get; set; } = string.Empty; //prevent null reference exception
        public string NameQuery { get; set; } = string.Empty; //prevent null reference exception
        public bool? AnalyticsEnabledOnly { get; set; }
        public bool IncludeSearcher { get; set; } = false;
    }
}
