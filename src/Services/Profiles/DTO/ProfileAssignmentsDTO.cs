using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileAssignmentsDTO
    {
        public int[] Segments { get; set; }
        public Guid? TeamId { get; set; }
    }
}
