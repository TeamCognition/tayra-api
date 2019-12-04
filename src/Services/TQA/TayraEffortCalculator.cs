using System;
using System.Collections.Generic;

namespace Tayra.Services
{
    public static class TayraEffortCalculator
    {
        private static double TimeFunction(double time, int story)
        {
            double w1 = 1;
            double w2 = 0.75d;
            double w3 = 0.66d;
 
            double BP1 = 4 * 60 * story; //8 hours if story = 2
            double BP2 = 4 * 60 * 3 * story; //3 days if story = 2
 
            if (time < BP1)
            {
                return time;
            }
            else if (time < BP2)
            {
                return BP1 * w1 + (time - BP1) * w2;
            }
            else
            {
                return BP1 * w1 + (BP2 - BP1) * w2 + (time - BP2) * w3;
            }
        }
 
        //time values are in minutes
        public static double CalcEffortScore(double time, int story)
        {
            return (TimeFunction(time, story) / (8.2972 + 1.65));
        }
    }
}