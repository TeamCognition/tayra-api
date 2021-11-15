using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class LogGridDTO
    {
        public LogEvents Event { get; set; }
        public object Data { get; set; }
        public DateTime Created { get; set; }
        public bool IsAssistedByTayra { get; set; }

        public string Message { get; set; } 
        
        public string Uri { get; set; } 
        
        public string ProfileAvatar { get; set; }

        public string ProfileFullName { get; set; }

        public string ProfileUsername { get; set; }
    }
}
