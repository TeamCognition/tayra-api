using System;
using System.Collections.Generic;
using System.Text;

namespace Tayra.Services
{
    public class TeamImpactLineChartDTO
    {
        public int StartDateId { get; set; }
        public int EndDateId { get; set; }

        public float[] Averages { get; set; }
    }
}
