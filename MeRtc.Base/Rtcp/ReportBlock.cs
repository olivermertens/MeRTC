namespace MeRtc.Base
{
    public class ReportBlock : IRtcpData
    {
        public const int SIZE = 24;
        public int Size => SIZE;

        uint Ssrc { get; set; }
        byte FractionLost { get; set; }
        int PacketsLost { get; set; }
        uint ExtendedHighestSequence { get; set; }
        uint Jitter { get; set; }
        uint LastSenderReport { get; set; }
        uint DelaySinceLastSenderReport { get; set; }

        public void ReadFromBuffer(byte[] buffer, int startIndex)
        {
            Ssrc = BufferConverter.ToUInt32(buffer, startIndex, true);
            FractionLost = buffer[4];
            PacketsLost = BufferConverter.ThreeBytesToInt(buffer, 5, true);
            ExtendedHighestSequence = BufferConverter.ToUInt32(buffer, 8, true);
            Jitter = BufferConverter.ToUInt32(buffer, 12, true);
            LastSenderReport = BufferConverter.ToUInt32(buffer, 16, true);
            DelaySinceLastSenderReport = BufferConverter.ToUInt32(buffer, 20, true);
        }

        public void WriteToBuffer(byte[] buffer, int startIndex)
        {
            BufferConverter.WriteBytes(Ssrc, buffer, startIndex, true);
            buffer[4] = FractionLost;
            BufferConverter.WriteIntToThreeBytes(PacketsLost, buffer, 5, true);
            BufferConverter.WriteBytes(ExtendedHighestSequence, buffer, 8, true);
            BufferConverter.WriteBytes(Jitter, buffer, 12, true);
            BufferConverter.WriteBytes(LastSenderReport, buffer, 16, true);
            BufferConverter.WriteBytes(DelaySinceLastSenderReport, buffer, 20, true);
        }
    }
}