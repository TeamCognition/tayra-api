using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class LogGridDTO
    {
        public LogEvents Event { get; set; }
        public object Data { get; set; }
        public DateTime Created { get; set; }
    }
}
