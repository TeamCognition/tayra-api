using System;
using System.Collections.Generic;

namespace Tayra.Services._Models.Functions
{
    public class Function
    {
        public Trigger Trigger { get; set; }
        public Action Action { get; set; }
    }

    public class Trigger
    {
        //public IEvent Event { get; set; }

        public Dictionary<string, string> Data { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class Action
    {
        public Option[] Options { get; set; }
        
        public class Data
        {
            
        }

        public class Option
        {
            public string Name { get; set; }

            public void Handler()
            {
                
            }
        }
    }
}