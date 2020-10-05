using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeRtc.Base
{
    public abstract class ReportPacket : RtcpPacket
    {
        public override int Length
        {
            get
            {
                int length = ReportBlocksStartPosition;
                if (reportBlocks != null)
                    length += reportBlocks.Count * ReportBlock.SIZE;
                return length;
            }
        }

        protected List<ReportBlock> reportBlocks;
        public ReadOnlyCollection<ReportBlock> ReportBlocks => reportBlocks?.AsReadOnly();
        protected abstract int ReportBlocksStartPosition { get; }

        protected ReportPacket(byte[] buffer, int offset)
            : base(buffer, offset) { }

        protected ReportPacket() { }

        public uint Ssrc
        {
            get { return BufferConverter.ToUInt32(Buffer, Offset + 4, true); }
            set { BufferConverter.WriteBytes(value, Buffer, Offset + 4); }
        }

        public void AddReportBlock(ReportBlock reportBlock)
        {
            if (reportBlocks == null)
                reportBlocks = new List<ReportBlock>();
            reportBlock.WriteToBuffer(Buffer, Offset + ReportBlocksStartPosition + reportBlocks.Count * ReportBlock.SIZE);
            reportBlocks.Add(reportBlock);
        }

        protected override void LoadFromBuffer(byte[] buffer, int offset)
        {
            Buffer = buffer;
            Offset = offset;
            int position = ReportBlocksStartPosition;
            if (PacketLength > position)
            {
                reportBlocks = new List<ReportBlock>();
                while (PacketLength >= position + ReportBlock.SIZE)
                {
                    var reportBlock = new ReportBlock();
                    reportBlock.ReadFromBuffer(buffer, position);
                    reportBlocks.Add(reportBlock);
                    position += ReportBlock.SIZE;
                }
            }
        }

        public override void Init()
        {
            base.Init();
            reportBlocks = null;
        }
    }
}