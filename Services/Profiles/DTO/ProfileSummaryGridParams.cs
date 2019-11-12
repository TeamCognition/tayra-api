using Firdaws.Core;

namespace Tayra.Services
{
    public class ProfileSummaryGridParams : GridParams
    {
        public string ProjectKeyQuery { get; set; } //TODO: without query, and ??
        public string NicknameQuery { get; set; } = string.Empty; //prevent null reference exception
        public string NameQuery { get; set; } = string.Empty; //prevent null reference exception
    }
}
