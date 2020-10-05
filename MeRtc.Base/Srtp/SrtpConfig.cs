using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class SrtpConfig
    {
        public static int MaxConsecutivePacketsDescardedEarly => -1;
        public static List<int> ProtectionProfiles => new List<int>()
        {
            SrtpProtectionProfile.SRTP_AEAD_AES_128_GCM,
            SrtpProtectionProfile.SRTP_AES128_CM_HMAC_SHA1_80,
            SrtpProtectionProfile.SRTP_AES128_CM_HMAC_SHA1_32
        };
        public static string FactoryClass => "BouncyCastle";
    }
}