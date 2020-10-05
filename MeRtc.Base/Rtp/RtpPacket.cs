using System;
using System.Collections;
using System.Collections.Generic;

namespace MeRtc.Base
{

    //TODO Consider max payload size.
    public class RtpPacket
    {
        public RtpHeader Header { get; private set; }
        public ArraySegment<byte> Payload { get; private set; }
        public int Length { get; }
        public int PayloadStartIndex => Header.Length;
        public byte[] Buffer { get; }

        private RtpPacket() { }

        public RtpPacket(byte[] buffer, int offset, PayloadType payloadType = null, uint ssrc = 0, ushort sequenceNumber = 0, uint timestamp = 0, bool marker = false, uint? extension = null, uint[] csrc = null)
        {
            Header = new RtpHeader(buffer, offset, payloadType, ssrc, sequenceNumber, timestamp, marker, extension, csrc);
        }

        public static RtpPacket FromBuffer(byte[] buffer, int offset, int? length = null)
        {
            var rtpPacket = new RtpPacket();
            rtpPacket.Header = RtpHeader.FromBuffer(buffer, offset);
            int payloadStartIndex = offset + rtpPacket.Header.Length;
            int payloadLength;
            if (length != null)
            {
                payloadLength = length.Value - payloadStartIndex;
            }
            else
            {
                // Check payload to determine its length.
                switch (rtpPacket.Header.PayloadType.Number)
                {
                    default:
                        payloadLength = buffer.Length - payloadStartIndex;
                        break;
                }
            }
            rtpPacket.Payload = new ArraySegment<byte>(buffer, payloadStartIndex, payloadLength);
            return rtpPacket;
        }
    }

    public class RtpHeader
    {

        /*
        The RTP header has the following format:

        0                   1                   2                   3
        0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
       |V=2|P|X|  CC   |M|     PT      |       sequence number         |
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
       |                           timestamp                           |
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
       |           synchronization source (SSRC) identifier            |
       +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
       |            contributing source (CSRC) identifiers             |
       |                             ....                              |
       +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        */

        public int Length { get; private set; }

        public byte[] Buffer { get; private set; }

        public int Offset { get; private set; }

        private RtpHeader() { }

        public RtpHeader(byte[] buffer, int offset, PayloadType payloadType = null, uint ssrc = 0, ushort sequenceNumber = 0, uint timestamp = 0, bool marker = false, uint? extension = null, uint[] csrc = null)
        {
            Buffer = buffer;
            Offset = offset;
            PayloadType = payloadType ?? PayloadType.Unknown;
            Ssrc = ssrc;
            SequenceNumber = sequenceNumber;

            Extension = extension;
            CsrcList = csrc;
        }

        ///<Summary>
        /// Indicates the version of the protocol. Current version is 2.
        ///</Summary>
        public int Version
        {
            get { return (Buffer[Offset + 0] & 0b11000000) >> 6; }
            internal set { Buffer[Offset + 0] = (byte)((value << 6) | (Buffer[Offset + 0] & 0b00111111)); }
        }

        ///<Summary>
        /// Used to indicate if there are extra padding bytes at the end of the RTP packet. The last byte of the padding contains the number of padding bytes that were added (including itself).
        ///</Summary>
        public bool Padding
        {
            get { return (Buffer[Offset + 0] & 0b00100000) != 0; }
            internal set { Buffer[Offset + 0] = (byte)(value ? (Buffer[Offset + 0] | 0b00100000) : (Buffer[Offset + 0] & 0b11011111)); }
        }

        ///<Summary>
        /// Indicates presence of an extension header between the header and payload data. The extension header is application or profile specific.
        ///</Summary>
        public bool HasExtension
        {
            get { return (Buffer[Offset + 0] & 0b00010000) != 0; }
            private set { Buffer[Offset + 0] = (byte)(value ? (Buffer[Offset + 0] | 0b00010000) : (Buffer[Offset + 0] & 0b11101111)); }
        }

        ///<Summary>
        /// Contains the number of CSRC identifiers that follow the SSRC.
        ///</Summary>
        public int CsrcCount
        {
            get { return (Buffer[Offset + 0] & 0b00001111); }
            private set { Buffer[Offset + 0] = (byte)((Buffer[Offset + 0] & 0b11110000) | value); }
        }

        ///<Summary>
        /// Signaling used at the application level in a profile-specific manner. If it is set, it means that the current data has some special relevance for the application.
        ///</Summary>
        public bool Marker
        {
            get { return (Buffer[Offset + 1] & 0b10000000) != 0; }
            internal set { Buffer[Offset + 1] = (byte)(value ? (Buffer[Offset + 1] | 0b10000000) : (Buffer[Offset + 1] & 0b01111111)); }
        }

        ///<Summary>
        /// Indicates the format of the payload and thus determines its interpretation by the application. Values are profile specific and may be dynamically assigned.
        ///</Summary>
        public PayloadType PayloadType
        {
            get { return PayloadType.GetPayloadType(Buffer[Offset + 1] & 0b01111111); }
            internal set { Buffer[Offset + 1] = (byte)((Buffer[Offset + 1] & 0b10000000) | value.Number); }
        }

        private ushort sequenceNumber;
        ///<Summary>
        /// The sequence number increments by one for each RTP data packet
        /// sent, and may be used by the receiver to detect packet loss and to
        /// restore packet sequence.  The initial value of the sequence number
        /// SHOULD be random (unpredictable) to make known-plaintext attacks
        /// on encryption more difficult.
        ///</Summary>
        public ushort SequenceNumber
        {
            get { return sequenceNumber; }
            internal set
            {
                sequenceNumber = value;
                Buffer[Offset + 2] = (byte)(sequenceNumber >> 8); Buffer[Offset + 3] = (byte)(sequenceNumber & 0xFF);
            }
        }

        private uint timestamp;
        ///<Summary>
        ///
        ///</Summary>
        public uint Timestamp
        {
            get { return timestamp; }
            internal set
            {
                timestamp = value;
                BufferConverter.WriteBytes(timestamp, Buffer, Offset + 4, true);
            }
        }

        ///<Summary>
        /// Synchronization source identifier uniquely identifies the source of a stream. The synchronization sources within the same RTP session will be unique.
        ///</Summary>
        public uint Ssrc
        {
            get { return BufferConverter.ToUInt32(Buffer, Offset + 8, true); }
            internal set { BufferConverter.WriteBytes(value, Buffer, Offset + 8, true); }
        }

        ///<Summary>
        /// Contributing source IDs enumerate contributing sources to a stream which has been generated from multiple sources.
        ///</Summary>
        public uint[] CsrcList
        {
            get
            {
                var list = new uint[CsrcCount];
                for (int i = 0; i < list.Length; i++)
                {
                    list[i] = BufferConverter.ToUInt32(Buffer, Offset + 12 + (i * 4), true);
                }
                return list;
            }
            private set
            {
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        BufferConverter.WriteBytes(value[i], Buffer, Offset + 12 + (i * 4), true);
                    }
                    CsrcCount = value.Length;
                }
                else
                    CsrcCount = 0;
            }
        }

        ///<Summary>
        /// The first 32-bit word contains a profile-specific identifier (16 bits) and a length specifier (16 bits) that indicates the length of the extension in 32-bit units, 
        /// excluding the 32 bits of the extension header. The extension header data follows.
        ///</Summary>
        public uint? Extension
        {
            get
            {
                if (HasExtension)
                    return BufferConverter.ToUInt32(Buffer, Offset + 12 + (CsrcCount * 4), true);
                else
                    return null;
            }
            private set
            {
                if (value != null)
                {
                    BufferConverter.WriteBytes(value.Value, Buffer, Offset + 12 + (CsrcCount * 4), true);
                    HasExtension = true;
                }
                else
                    HasExtension = false;
            }
        }

        public static RtpHeader FromBuffer(byte[] buffer, int offset)
        {

            var header = new RtpHeader();
            header.Buffer = buffer;
            header.Offset = offset;
            header.Length = 12; // Set minimal header length.

            // Check if Extension Bit is set.
            if (header.HasExtension)
                header.Length += 4;

            // Check CSRC
            header.Length += header.CsrcCount * 4;
            return header;
        }
    }
}
