using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class LogGridDTO
    {
        public LogEvents Event { get; set; }
        public object Data { get; set; }
        public DateTime Created { get; set; }
        public bool IsGuidedByTayra { get; set; }

        public string Description { get; set; } 
        
        public string DescriptionLink { get; set; } 
        
        public string AuthorAvatar { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUsername { get; set; }
    }
}
