using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeRtc.Base
{
    public class SenderReportPacket : ReportPacket
    {
        protected override PacketType DefaultPacketType => PacketType.SR;
        protected override int ReportBlocksStartPosition => Offset + HEADER_SIZE + 4 + SenderInfo.SIZE;

        public void UpdateSenderInfo(SenderInfo senderInfo)
        {
            senderInfo.WriteToBuffer(Buffer, Offset + HEADER_SIZE);
        }

        public SenderInfo GetSenderInfo()
        {
            var senderInfo = new SenderInfo();
            senderInfo.ReadFromBuffer(Buffer, Offset + HEADER_SIZE);
            return senderInfo;
        }

        internal SenderReportPacket(){}

        public SenderReportPacket(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Init();
        }

        public SenderReportPacket(byte[] buffer, int offset, uint ssrc)
            : this(buffer, offset)
        {
            Ssrc = ssrc;
        }
    }
}