using System;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class DtlsServer : IDtlsRole
    {
        public event EventHandler<HandshakeCompleteEventArgs> HandshakeComplete;
        public event EventHandler<Certificate> ClientCertificateAvailable;

        DatagramTransport DatagramTransport { get; }
        TlsServerImpl tlsServer;
        DtlsServerProtocol dtlsServerProtocol;

        public DtlsServer(DatagramTransport datagramTransport, CertificateInfo certificateInfo, DtlsServerProtocol dtlsServerProtocol = null)
        {
            DatagramTransport = datagramTransport;
            this.dtlsServerProtocol = dtlsServerProtocol ?? new DtlsServerProtocol(new Org.BouncyCastle.Security.SecureRandom());
            tlsServer = new TlsServerImpl(certificateInfo);
            tlsServer.ClientCertificateReceived += TlsServer_ClientCertificateReceived;
        }

        public DtlsTransport Start() => Accept();

        public DtlsTransport Accept()
        {
            try
            {
                DtlsTransport dtlsTransport = dtlsServerProtocol.Accept(tlsServer, DatagramTransport);
                HandshakeComplete?.Invoke(this, new HandshakeCompleteEventArgs(tlsServer.ChosenSrtpProtectionProfile, TlsRole.Server, tlsServer.SrtpKeyingMaterial));
                return dtlsTransport;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        private void TlsServer_ClientCertificateReceived(object sender, Certificate e)
        {
            ClientCertificateInfoAvailable?.Invoke(this, e);
        }
    }
}