using System.Runtime.CompilerServices;
using System.Security;

namespace Cog.Core
{
    /// <summary>
    /// Indicates that parameter tampering has been made in order to make intrusion in system consistency.
    /// </summary>
    public class CogSecurityException : SecurityException
    {
        /// <summary>
        /// Name of the method where intrusion was detected.
        /// </summary>
        public string MethodName => Data["MethodName"] as string;

        /// <summary>
        /// Identifier of the identity within security context.
        /// </summary>
        public int IdentityId => (Data["IdentityId"] as int?) ?? 0;

        /// <summary>
        /// Email-address of the identity within security context.
        /// </summary>
        public string IdentityUserName => Data["IdentityUserName"] as string;

        /// <summary>
        /// Query string of the request.
        /// </summary>
        public string RequestQueryString => Data["RequestQueryString"] as string;

        /// <summary>
        /// Body of the request. Logged only if content type is json or form-urlencoded and body contents do not exceed 1024 kB.
        /// </summary>
        public string RequestBody => Data["RequestBody"] as string;

        public CogSecurityException(string reason, [CallerMemberName] string methodName = "")
            : base(reason)
        {
            Data.Add("MethodName", methodName);
        }
    }
}
