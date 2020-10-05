using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto.Digests;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace MeRtc.Base
{
    public static class DtlsUtils
    {
        public static CertificateInfo GenerateCertificateInfo()
        {
            var cn = GenerateCN("AppName", "0.01");
            var keyPair = GenerateEcKeyPair();
            var x509Certificate = GenerateCertificate(cn, keyPair);
            var localFingerprint = GetFingerprint(x509Certificate, out string localFingerprintHashFunction);

            var certificate = new Certificate(new X509CertificateStructure[] { x509Certificate.CertificateStructure });
            return new CertificateInfo(keyPair, certificate, localFingerprintHashFunction, localFingerprint, DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        private static X509Certificate GenerateCertificate(X509Name subject, AsymmetricCipherKeyPair keyPair)
        {
            X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();
            var now = System.DateTime.Now;
            var startDate = now.AddDays(-1);
            var expiryDate = now.AddDays(7);
            BigInteger serialNumber = BigInteger.ValueOf(new DateTimeOffset(now).ToUnixTimeMilliseconds());

            certificateGenerator.SetIssuerDN(subject);
            certificateGenerator.SetSerialNumber(serialNumber);
            certificateGenerator.SetNotBefore(startDate);
            certificateGenerator.SetNotAfter(expiryDate);
            certificateGenerator.SetSubjectDN(subject);
            certificateGenerator.SetPublicKey(keyPair.Public);

            ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256withECDSA", keyPair.Private);

            return certificateGenerator.Generate(signatureFactory);
        }

        private static AsymmetricCipherKeyPair GenerateEcKeyPair()
        {
            var curveNames = ECNamedCurveTable.Names;
            var ecCurveSpec = ECNamedCurveTable.GetByName("secp256r1");
            var keyGen = new ECKeyPairGenerator();
            keyGen.Init(new ECKeyGenerationParameters(new ECDomainParameters(ecCurveSpec.Curve, ecCurveSpec.G, ecCurveSpec.N, ecCurveSpec.H, ecCurveSpec.GetSeed()), new SecureRandom()));
            return keyGen.GenerateKeyPair();
        }

        private static X509Name GenerateCN(string appName, string appVersion)
        {
            return new X509Name("CN=" + appName + appVersion);
        }

        private static string GetFingerprint(X509Certificate certificate, out string hashFunctionName)
        {
            var algorithms = DigestUtilities.Algorithms;
            var digAlgId = new DefaultDigestAlgorithmIdentifierFinder().find(certificate.CertificateStructure.SignatureAlgorithm);
            IDigest digest = DigestUtilities.GetDigest(digAlgId.Algorithm);
            byte[] input = certificate.GetEncoded();
            digest.BlockUpdate(input, 0, input.Length);
            byte[] output = new byte[digest.GetDigestSize()];
            digest.DoFinal(output, 0);

            hashFunctionName = digest.AlgorithmName;
            return BytesToFingerprintString(output);
        }

        private static string BytesToFingerprintString(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(((bytes[i] & 0xF0) >> 4).ToString("X"));
                sb.Append((bytes[i] & 0x0F).ToString("X"));
                if (i < bytes.Length - 1)
                    sb.Append(':');
            }
            return sb.ToString();
        }

        internal static int ChooseSrtpProtectionProfile(ICollection<int> ours, ICollection<int> theirs)
        {
            try
            {
                return ours.First((o) => theirs.Contains(o));
            }
            catch(InvalidOperationException e)
            {
                throw e;
            }
        }
    }
}