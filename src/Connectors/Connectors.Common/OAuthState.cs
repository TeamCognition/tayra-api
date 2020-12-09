using System;
using Cog.Core;

namespace Tayra.Connectors.Common
{
    public class OAuthState
    {
        public string TenantKey { get; }
        public Guid ProfileId { get; }
        public Guid SegmentId { get; }
        public bool IsSegmentAuth { get; }
        public string ReturnPath { get; }

        public OAuthState(string tenantKey, Guid profileId, Guid segmentId, bool isSegmentAuth, string returnPath)
        {
            ProfileId = profileId;
            SegmentId = segmentId;
            IsSegmentAuth = isSegmentAuth;
            ReturnPath = returnPath;
            TenantKey = tenantKey;
        }

        public OAuthState(string oAuthState)
        {
            try
            {
                var stateProps = SplitProps(Cipher.Decrypt(oAuthState.Base64UrlDecode()));
                ProfileId = Guid.Parse(stateProps[0]);
                SegmentId = Guid.Parse(stateProps[1]);
                IsSegmentAuth = bool.Parse(stateProps[2]);
                TenantKey = stateProps[3];
                ReturnPath = stateProps[4];
            }
            catch (Exception)
            {
                throw new ApplicationException("could not parse oAuthString");
            }
        }

        public override string ToString() =>
            Cipher.Encrypt(JoinProps())
                .Base64UrlEncode();


        private const char _separator = '|';
        private string JoinProps() => string.Join(_separator, ProfileId, SegmentId, IsSegmentAuth, TenantKey, ReturnPath);
        private string[] SplitProps(string joined) => joined.Split(_separator);
    }
}