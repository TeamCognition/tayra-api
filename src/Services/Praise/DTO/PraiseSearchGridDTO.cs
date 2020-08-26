using System;
using System.Collections.Generic;
using System.Text;
using Tayra.Common;

namespace Tayra.Services
{
    public class PraiseSearchGridDTO
    { 
        public string RecieverUsername { get; set; }
        public string PraiserUsername { get; set; }
        public string RecieverAvatar { get; set; }
        public DateTime Created { get; set; }
        public PraiseTypes Type { get; set; }
        public string Message { get; set; }

    }
}
