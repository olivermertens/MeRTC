using System;

namespace MeRtc.Base
{
    // https://tools.ietf.org/html/rfc3550#section-6.4.1
    public class SenderInfo : IRtcpData
    {
        public const int SIZE = 20;
        public int Size => SIZE;

        public ulong NtpTimestamp { get; set; }
        public uint RtpTimestamp { get; set; }
        public uint PacketCount { get; set; }
        public uint OctetCount { get; set; }

        public SenderInfo() { }
        public SenderInfo(ulong ntpTimestamp, uint rtpTimestamp, uint packetCount, uint octetCount)
        {
            NtpTimestamp = ntpTimestamp;
            RtpTimestamp = rtpTimestamp;
            PacketCount = packetCount;
            OctetCount = octetCount;
        }

        public void ReadFromBuffer(byte[] buffer, int startIndex)
        {
            NtpTimestamp = BitConverter.ToUInt64(buffer, startIndex);
            RtpTimestamp = BitConverter.ToUInt32(buffer, startIndex + 8);
            PacketCount = BitConverter.ToUInt32(buffer, startIndex + 12);
            OctetCount = BitConverter.ToUInt32(buffer, startIndex + 16);
            return;
        }

        public void WriteToBuffer(byte[] buffer, int startIndex)
        {
            BufferConverter.WriteBytes(NtpTimestamp, buffer, startIndex, true);
            BufferConverter.WriteBytes(RtpTimestamp, buffer, startIndex + 8, true);
            BufferConverter.WriteBytes(PacketCount, buffer, startIndex + 12, true);
            BufferConverter.WriteBytes(OctetCount, buffer, startIndex + 16, true);
            return;
        }
    }
}