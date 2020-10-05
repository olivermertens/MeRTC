namespace MeRtc.Base
{
    public interface IRtcpData
    {
        void WriteToBuffer(byte[] buffer, int startIndex);
        void ReadFromBuffer(byte[] buffer, int startIndex);
        int Size { get; }
    }
}