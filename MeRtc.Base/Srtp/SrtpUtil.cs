using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public static class SrtpUtil
    {
        public static SrtpProfileInformation GetSrtpProfileInformationFromSrtpProtectionProfile(int srtpProtectionProfile)
        {
            switch (srtpProtectionProfile)
            {
                case SrtpProtectionProfile.SRTP_AES128_CM_HMAC_SHA1_32:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 128 / 8,
                        cipherSaltLength: 112 / 8,
                        cipherName: SrtpPolicy.AESCM_ENCRYPTION,
                        authFunctionName: SrtpPolicy.HMACSHA1_AUTHENTICATION,
                        authKeyLength: 160 / 8,
                        rtcpAuthTagLength: 80 / 8,
                        rtpAuthTagLength: 32 / 8
                    );
                case SrtpProtectionProfile.SRTP_AES128_CM_HMAC_SHA1_80:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 128 / 8,
                        cipherSaltLength: 112 / 8,
                        cipherName: SrtpPolicy.AESCM_ENCRYPTION,
                        authFunctionName: SrtpPolicy.HMACSHA1_AUTHENTICATION,
                        authKeyLength: 160 / 8,
                        rtcpAuthTagLength: 80 / 8,
                        rtpAuthTagLength: 80 / 8
                    );
                case SrtpProtectionProfile.SRTP_NULL_HMAC_SHA1_32:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 0,
                        cipherSaltLength: 0,
                        cipherName: SrtpPolicy.NULL_ENCRYPTION,
                        authFunctionName: SrtpPolicy.HMACSHA1_AUTHENTICATION,
                        authKeyLength: 160 / 8,
                        rtcpAuthTagLength: 80 / 8,
                        rtpAuthTagLength: 32 / 8
                    );
                case SrtpProtectionProfile.SRTP_NULL_HMAC_SHA1_80:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 0,
                        cipherSaltLength: 0,
                        cipherName: SrtpPolicy.NULL_ENCRYPTION,
                        authFunctionName: SrtpPolicy.HMACSHA1_AUTHENTICATION,
                        authKeyLength: 160 / 8,
                        rtcpAuthTagLength: 80 / 8,
                        rtpAuthTagLength: 80 / 8
                    );
                case SrtpProtectionProfile.SRTP_AEAD_AES_128_GCM:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 128 / 8,
                        cipherSaltLength: 96 / 8,
                        cipherName: SrtpPolicy.AESGCM_ENCRYPTION,
                        authFunctionName: SrtpPolicy.NULL_AUTHENTICATION,
                        authKeyLength: 0,
                        rtcpAuthTagLength: 128 / 8,
                        rtpAuthTagLength: 128 / 8
                    );
                case SrtpProtectionProfile.SRTP_AEAD_AES_256_GCM:
                    return new SrtpProfileInformation(
                        cipherKeyLength: 256 / 8,
                        cipherSaltLength: 96 / 8,
                        cipherName: SrtpPolicy.AESGCM_ENCRYPTION,
                        authFunctionName: SrtpPolicy.NULL_AUTHENTICATION,
                        authKeyLength: 0,
                        rtcpAuthTagLength: 128 / 8,
                        rtpAuthTagLength: 128 / 8
                    );
                default:
                    throw new System.ArgumentException("Unsupported SRTP protection profile: " + srtpProtectionProfile);
            }
        }
    }
}