using Firdaws.Core;

namespace Tayra.Services
{
    public class ProfileGridParams : GridParams
    {
        //public string SegmentKeyQuery { get; set; } //TODO: without query, and ??
        public string UsernameQuery { get; set; } = string.Empty; //prevent null reference exception
        public string NameQuery { get; set; } = string.Empty; //prevent null reference exception
    }
}
