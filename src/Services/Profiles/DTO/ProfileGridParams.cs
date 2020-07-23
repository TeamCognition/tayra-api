﻿using Cog.Core;

namespace Tayra.Services
{
    public class ProfileGridParams : GridParams
    {
        public int? SegmentIdExclude { get; set; }
        public string UsernameQuery { get; set; } = string.Empty; //prevent null reference exception
        public string NameQuery { get; set; } = string.Empty; //prevent null reference exception
        public bool IncludeAdmins { get; set; } = false;
        public bool IncludeManagers { get; set; } = false;
    }
}
