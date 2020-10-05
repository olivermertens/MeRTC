using System;

namespace MeRtc.Base
{
    public enum PacketType : byte
    {
        SR = 200, RR = 201, SDES = 202, BYE = 203, APP = 204
    }

    public abstract class RtcpPacket
    {
        protected abstract PacketType DefaultPacketType { get; }
        /* Header
        0                   1                   2                   3
        0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
       |V=2|P|    IC   |      PT       |             length            |
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        */
        public const int HEADER_SIZE = 4;


        public abstract int Length { get; }

        public byte[] Buffer { get; protected set; }

        public int Offset { get; protected set; }

        ///<Summary>
        /// Identifies the version of RTP, which is the same in RTCP packets
        /// as in RTP data packets.  The version defined by this specification
        /// is two (2).
        ///</Summary>
        public int Version
        {
            get { return (Buffer[Offset + 0] & 0b11000000) >> 6; }
            internal set { Buffer[Offset + 0] = (byte)((value << 6) | (Buffer[Offset + 0] & 0b00111111)); }
        }

        ///<Summary>
        /// If the padding bit is set, this individual RTCP packet contains
        /// some additional padding octets at the end which are not part of
        /// the control information but are included in the length field.  The
        /// last octet of the padding is a count of how many padding octets
        /// should be ignored, including itself (it will be a multiple of
        /// four).
        ///</Summary>
        public bool Padding
        {
            get { return (Buffer[Offset + 0] & 0b00100000) != 0; }
            internal set { Buffer[Offset + 0] = (byte)(value ? (Buffer[Offset + 0] | 0b00100000) : (Buffer[Offset + 0] & 0b11011111)); }
        }

        ///<Summary>
        /// The number of reception report blocks contained in this packet.  A
        /// value of zero is valid.
        ///</Summary>
        public int ItemCount
        {
            get { return (Buffer[Offset + 0] & 0b00011111); }
            internal set { Buffer[Offset + 0] = (byte)((value & 0b00011111) | (Buffer[Offset + 0] & 0b11100000)); }
        }

        ///<Summary>
        /// Contains a constant to identify RTCP packet type.
        ///</Summary>
        public PacketType PacketType
        {
            get { return GetPacketType(Buffer, Offset); }
            internal set { Buffer[Offset + 1] = (byte)value; }
        }

        public static PacketType GetPacketType(byte[] buffer, int packetStartIndex)
        {
            return (PacketType)buffer[packetStartIndex + 1];
        }

        ///<Summary>
        /// Indicates the length of this RTCP packet.
        ///</Summary>
        public ushort PacketLength
        {
            get { return (ushort)(Buffer[Offset + 2] << 8 | Buffer[Offset + 3]); }
            internal set { Buffer[Offset + 2] = (byte)(value >> 8); Buffer[Offset + 3] = (byte)(value & 0xFF); }
        }

        public static T FromBuffer<T> (byte[] buffer, int offset) where T : RtcpPacket, new()
        {
            T obj = new T();
            obj.LoadFromBuffer(buffer, offset);
            return obj;
        }

        protected abstract void LoadFromBuffer(byte[] buffer, int offset);

        protected RtcpPacket(){}

        public RtcpPacket(byte[] buffer, int offset)
        {
            Buffer = buffer;
            Offset = offset;
        }

        public virtual void Init()
        {
            Version = 2;
            Padding = false;
            ItemCount = 0;
            PacketType = DefaultPacketType;
            PacketLength = 0;
        }
    }
}