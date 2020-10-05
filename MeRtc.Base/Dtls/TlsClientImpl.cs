using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class TlsClientImpl : DefaultTlsClient
    {
        public event EventHandler<Certificate> ServerCertificateInfoAvailable;
        public override ProtocolVersion ClientVersion => ProtocolVersion.DTLSv12;
        public override ProtocolVersion MinimumVersion => ProtocolVersion.DTLSv10;

        public byte[] SrtpKeyingMaterial { get; private set; }
        public int ChosenSrtpProtectionProfile { get; private set; } = 0;

        private CertificateInfo CertificateInfo { get; }

        private TlsSession session;
        private TlsCredentials clientCredentials;

        public TlsClientImpl(CertificateInfo certificateInfo)
            : base(/*BC_TLS_CRYPTO*/)
        {
            CertificateInfo = certificateInfo;
        }

        public override TlsSession GetSessionToResume() => session;

        public override TlsAuthentication GetAuthentication()
        {
            return new TlsAuthenticationImplementor
            (
                getClientCredentials: (certRequest) =>
                {
                    if (clientCredentials == null)
                    {
                        clientCredentials = new DefaultTlsSignerCredentials(mContext,
                                                                            CertificateInfo.certificate,
                                                                            CertificateInfo.keyPair.Private,
                                                                            new SignatureAndHashAlgorithm(HashAlgorithm.sha256, SignatureAlgorithm.ecdsa));
                    }
                    return clientCredentials;
                },
                notifyServerCertificate: (cert) => { ServerCertificateInfoAvailable?.Invoke(this, cert); }
            );
        }

        public override IDictionary GetClientExtensions()
        {
            var clientExtensions = base.GetClientExtensions();
            if (TlsSRTPUtils.GetUseSrtpExtension(clientExtensions) == null)
            {
                if (clientExtensions == null)
                    clientExtensions = new Dictionary<int, byte[]>();

                TlsSRTPUtils.AddUseSrtpExtension(clientExtensions, new UseSrtpData(SrtpConfig.ProtectionProfiles.ToArray(), TlsUtilities.EmptyBytes));
            }
            clientExtensions[ExtensionType.renegotiation_info] = new byte[] { 0 };
            return clientExtensions;
        }

        public override void ProcessServerExtensions(IDictionary serverExtensions)
        {
            base.ProcessServerExtensions(serverExtensions);
            var useSrtpData = TlsSRTPUtils.GetUseSrtpExtension(serverExtensions);
            var protectionProfiles = useSrtpData.ProtectionProfiles;
            ChosenSrtpProtectionProfile = DtlsUtils.ChooseSrtpProtectionProfile(SrtpConfig.ProtectionProfiles, protectionProfiles);
        }

        public override int[] GetCipherSuites()
        {
            return new int[] { CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256, CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA };
        }

        public override void NotifyHandshakeComplete()
        {
            base.NotifyHandshakeComplete();
            if (mContext.ResumableSession != null)
            {
                if (session != null && session.SessionID.SequenceEqual(mContext.ResumableSession.SessionID))
                {
                    // Resume.
                }
                else
                {
                    // New session.
                    session = mContext.ResumableSession;
                }
            }
            var srtpProfileInformation = SrtpUtil.GetSrtpProfileInformationFromSrtpProtectionProfile(ChosenSrtpProtectionProfile);
            SrtpKeyingMaterial = mContext.ExportKeyingMaterial(ExporterLabel.dtls_srtp, null, 2 * (srtpProfileInformation.CipherKeyLength + srtpProfileInformation.CipherSaltLength));
        }

    }
}