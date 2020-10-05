using System.Net.Sockets;
using Org.BouncyCastle.Crypto.Tls;

namespace MeRtc.Base
{
    public class DatagramTransportImpl : DatagramTransport
    {
        Socket udpSocket;

        public DatagramTransportImpl(Socket udpSocket)
        {
            this.udpSocket = udpSocket;
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {
            if (udpSocket.Poll(waitMillis, SelectMode.SelectRead))
                return udpSocket.Receive(buf, off, len, SocketFlags.None);
            else
                return -1;
        }

        public void Send(byte[] buf, int off, int len)
        {
            udpSocket.Send(buf, off, len, SocketFlags.None);
        }

        public void Close()
        {
            udpSocket.Close();
        }

        public int GetReceiveLimit() => 1500 - 20 - 8;

        public int GetSendLimit() => 1500 - 84 - 8;

    }
}