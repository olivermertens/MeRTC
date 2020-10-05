using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base.Test
{
    [TestClass]
    public class DtlsConnectionTest
    {
        [TestMethod]
        public void ConnectTest()
        {
            //CertificateInfo certificateInfo = DtlsUtils.
            byte[] testData = {1, 2, 3, 4, 5};

            int clientUdpPort = 8001, serverUdpPort = 8002;
            var clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Any, clientUdpPort));

            var serverSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, serverUdpPort));

            clientSocket.Connect(IPAddress.Loopback, serverUdpPort);
            serverSocket.Connect(IPAddress.Loopback, clientUdpPort);

            clientSocket.Send(testData);
            byte[] receiveBuffer = new byte[serverSocket.Available];
            serverSocket.Receive(receiveBuffer);

            CollectionAssert.AreEqual(receiveBuffer, testData);

            CertificateInfo clientCertificateInfo = DtlsUtils.GenerateCertificateInfo();
            var clientDatagramTransport = new DatagramTransportImpl(clientSocket);
            var client = new DtlsClient(clientDatagramTransport, clientCertificateInfo, new DtlsClientProtocol(new Org.BouncyCastle.Security.SecureRandom()));

            CertificateInfo serverCertificateInfo = DtlsUtils.GenerateCertificateInfo();
            var serverDatagramTransport = new DatagramTransportImpl(serverSocket);
            var server = new DtlsServer(serverDatagramTransport, serverCertificateInfo, new DtlsServerProtocol(new Org.BouncyCastle.Security.SecureRandom()));

            Task serverAcceptTask = new Task(() => server.Start());
            serverAcceptTask.Start();       

            client.HandshakeComplete += (sender, args) =>
            {

            };
            client.ServerCertificateAvailable += (sender, args) =>
            {

            };
            
            client.Start();

            
        }

        [TestMethod]
        public void CertificateInfoGenerationTest()
        {
            var certInfo = DtlsUtils.GenerateCertificateInfo();
        }
    }
}