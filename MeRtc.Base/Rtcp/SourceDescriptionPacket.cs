using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeRtc.Base
{
    public class SourceDescriptionPacket : RtcpPacket
    {
        protected override PacketType DefaultPacketType => PacketType.SDES;
        public override int Length => endPosition - Offset;

        int endPosition = HEADER_SIZE;

        List<SdesData> sdesDataList;
        public ReadOnlyCollection<SdesData> SdesData => sdesDataList.AsReadOnly();

        public SourceDescriptionPacket()
        {
            sdesDataList = new List<SdesData>();
        }

        public void AddSdesData(SdesData sdesData)
        {
            sdesDataList.Add(sdesData);
            sdesData.WriteToBuffer(Buffer, endPosition);
            endPosition += sdesData.Size;
        }

        protected override void LoadFromBuffer(byte[] buffer, int offset)
        {
            Buffer = buffer;
            Offset = offset;
            sdesDataList.Clear();
            int position = HEADER_SIZE;
            while(PacketLength > position)
            {
                var sdes = new SdesData();
                sdes.ReadFromBuffer(buffer, Offset + position);
                sdesDataList.Add(sdes);
                position += sdes.Size;
            }
        }
    }
}