using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ClaimBundleViewGridDTO
    {
        public int Id { get; set; }
        public ClaimBundleTypes Type { get; set; }

        public DateTime Created  { get; set; }
    }
}
