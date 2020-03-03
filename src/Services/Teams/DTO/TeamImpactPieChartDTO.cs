using System;
using System.Collections.Generic;
using System.Text;

namespace Tayra.Services
{
    public class TeamImpactPieChartDTO
    {
        public ProfilesDTO[] Profiles { get; set; }

        public class ProfilesDTO
        {
            public string Username { get; set; }
            public float ImpactPercentage { get; set; }
        }
    }
}
