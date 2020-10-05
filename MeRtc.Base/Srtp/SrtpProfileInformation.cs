namespace MeRtc.Base
{
    public class SrtpProfileInformation
    {
        public int CipherKeyLength { get; }
        public int CipherSaltLength { get; }
        public int CipherName { get; }
        public int AuthFunctionName { get; }
        public int AuthKeyLength { get; }
        public int RtcpAuthTagLength { get; }
        public int RtpAuthTagLength { get; }
        
         public SrtpProfileInformation(int cipherKeyLength, int cipherSaltLength, int cipherName, int authFunctionName, int authKeyLength, int rtcpAuthTagLength, int rtpAuthTagLength)
        {
            CipherKeyLength = cipherKeyLength;
            CipherSaltLength = cipherSaltLength;
            CipherName = cipherName;
            AuthFunctionName = authFunctionName;
            AuthKeyLength = authKeyLength;
            RtcpAuthTagLength = rtcpAuthTagLength;
            RtpAuthTagLength = rtpAuthTagLength;
        }
    }
}