using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class CertificateInfo
    {

        public AsymmetricCipherKeyPair keyPair { get; }
        public Certificate certificate { get; }
        public string localFingerprintHashFunction { get; }
        public string localFingerprint { get; }
        public long creationTimestampMs { get; }

        public CertificateInfo(AsymmetricCipherKeyPair keyPair, Certificate certificate, string localFingerprintHashFunction, string localFingerprint, long creationTimestampMs)
        {
            this.keyPair = keyPair;
            this.certificate = certificate;
            this.localFingerprintHashFunction = localFingerprintHashFunction;
            this.localFingerprint = localFingerprint;
            this.creationTimestampMs = creationTimestampMs;
        }
    }
}