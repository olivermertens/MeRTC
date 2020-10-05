using System;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;

namespace MeRtc.Base
{
    public class DtlsClient : IDtlsRole, IDisposable
    {
        public event EventHandler<HandshakeCompleteEventArgs> HandshakeComplete;
        public event EventHandler<Certificate> ServerCertificateAvailable;

        public DtlsClientProtocol dtlsClientProtocol { get; }

        DatagramTransport datagramTransport;
        TlsClientImpl tlsClient;
        bool isDisposed = false;


        public DtlsClient(DatagramTransport datagramTransport, CertificateInfo certificateInfo, DtlsClientProtocol dtlsClientProtocol = null)
        {
            this.datagramTransport = datagramTransport;
            this.dtlsClientProtocol = dtlsClientProtocol ?? new DtlsClientProtocol(new SecureRandom());
            tlsClient = new TlsClientImpl(certificateInfo);
            tlsClient.ServerCertificateInfoAvailable += TlsClient_ServerCertificateInfoAvailable;
        }

        ~DtlsClient() => Dispose();

        public DtlsTransport Start() => Connect();

        public void Dispose()
        {
            if(!isDisposed)
            {
                tlsClient.ServerCertificateInfoAvailable -= TlsClient_ServerCertificateInfoAvailable;
            }
        }

        DtlsTransport Connect()
        {
            try
            {
                DtlsTransport dtlsTransport = dtlsClientProtocol.Connect(tlsClient, datagramTransport);
                HandshakeComplete?.Invoke(this, new HandshakeCompleteEventArgs(tlsClient.ChosenSrtpProtectionProfile, TlsRole.Client, tlsClient.SrtpKeyingMaterial));
                return dtlsTransport;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void TlsClient_ServerCertificateInfoAvailable(object sender, Certificate e)
        {
            ServerCertificateInfoAvailable?.Invoke(this, e);
        }
    }
}