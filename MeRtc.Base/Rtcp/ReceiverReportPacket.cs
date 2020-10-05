namespace MeRtc.Base
{
    public class ReceiverReportPacket : ReportPacket
    {
        protected override PacketType DefaultPacketType => PacketType.RR;
        protected override int ReportBlocksStartPosition => HEADER_SIZE + 4; //Header + SSRC

        internal ReceiverReportPacket(){}

        public ReceiverReportPacket(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Init();
        }

        public ReceiverReportPacket(byte[] buffer, int offset, uint ssrc)
            : this(buffer, offset)
        {
            Ssrc = ssrc;
        }
    }
}