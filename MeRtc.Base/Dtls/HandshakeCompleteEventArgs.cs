using System;

namespace MeRtc.Base
{
    public enum TlsRole
    {
        Client, Server
    }

    public class HandshakeCompleteEventArgs : EventArgs
    {
        public int SrtpConnectionProfile { get; }
        public TlsRole TlsRole { get; }
        public byte[] SrtpKeyingMaterial { get; }

        public HandshakeCompleteEventArgs(int srtpConnectionProfile, TlsRole tlsRole, byte[] srtpKeyingMaterial)
        {
            SrtpConnectionProfile = srtpConnectionProfile;
            TlsRole = tlsRole;
            SrtpKeyingMaterial = srtpKeyingMaterial;
        }
    }
}