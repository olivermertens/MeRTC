using System;
using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class TlsServerImpl : DefaultTlsServer
    {
        public event EventHandler<Certificate> ClientCertificateReceived;
        public override ProtocolVersion GetServerVersion() => ProtocolVersion.DTLSv12;
        public byte[] SrtpKeyingMaterial { get; private set; }
        public int ChosenSrtpProtectionProfile { get; private set; } = 0;

        private TlsSession session;
        private CertificateInfo certificateInfo;

        public TlsServerImpl(CertificateInfo certificateInfo)
        {
            this.certificateInfo = certificateInfo;
            mServerVersion = ProtocolVersion.DTLSv12;
        }

        public override IDictionary GetServerExtensions()
        {
            var extensions = base.GetServerExtensions() ?? new Dictionary<int, byte[]>();
            if (TlsSRTPUtils.GetUseSrtpExtension(extensions) == null)
            {
                TlsSRTPUtils.AddUseSrtpExtension(extensions, new UseSrtpData(new int[] { ChosenSrtpProtectionProfile }, TlsUtilities.EmptyBytes));
            }
            return extensions;
        }

        public override void ProcessClientExtensions(IDictionary clientExtensions)
        {
            base.ProcessClientExtensions(clientExtensions);
            var useSrtpData = TlsSRTPUtils.GetUseSrtpExtension(clientExtensions);
            var protectionProfiles = useSrtpData.ProtectionProfiles;
            ChosenSrtpProtectionProfile = DtlsUtils.ChooseSrtpProtectionProfile(SrtpConfig.ProtectionProfiles, protectionProfiles);
        }

        protected override int[] GetCipherSuites()
        {
            return new int[] { CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256, CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA };
        }

        protected override TlsEncryptionCredentials GetRsaEncryptionCredentials()
        {
            return new DefaultTlsEncryptionCredentials(mContext, certificateInfo.certificate, certificateInfo.keyPair.Private);
        }

        protected override TlsSignerCredentials GetECDsaSignerCredentials()
        {
            return new DefaultTlsSignerCredentials(mContext, certificateInfo.certificate, certificateInfo.keyPair.Private, new SignatureAndHashAlgorithm(HashAlgorithm.sha256, SignatureAlgorithm.ecdsa));
        }

        public override CertificateRequest GetCertificateRequest()
        {
            var signatureAlgorithms = new List<SignatureAndHashAlgorithm>();
            signatureAlgorithms.Add(new SignatureAndHashAlgorithm(HashAlgorithm.sha256, SignatureAlgorithm.ecdsa));
            signatureAlgorithms.Add(new SignatureAndHashAlgorithm(HashAlgorithm.sha1, SignatureAlgorithm.rsa));

            if (mClientVersion == ProtocolVersion.DTLSv10)
                return new CertificateRequest(new byte[] { ClientCertificateType.rsa_sign }, null, null);
            else if (mClientVersion == ProtocolVersion.DTLSv12)
                return new CertificateRequest(new byte[] { ClientCertificateType.ecdsa_sign }, signatureAlgorithms, null);
            else
                throw new Exception("Unsupported version: " + mClientVersion);
        }

        public override void NotifyClientCertificate(Certificate clientCertificate)
        {
            ClientCertificateReceived?.Invoke(this, clientCertificate);
        }

        public override void NotifyHandshakeComplete()
        {
            base.NotifyHandshakeComplete();
        }
    }
}