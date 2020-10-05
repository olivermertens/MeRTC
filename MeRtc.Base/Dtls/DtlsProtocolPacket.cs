namespace MeRtc.Base
{
    public class DtlsProtocolPacket
    {
        public byte[] Buffer { get; }
        public int Offset { get; }
        public int Length { get; }

        public DtlsProtocolPacket(byte[] buffer, int offset, int length)
        {
            Buffer = buffer;
            Offset = offset;
            Length = length;
        }
    }
}