using System;
using System.Net;

namespace MeRtc.Base
{
    public class RtpSession
    {
        [Flags]
        public enum RtpOperationMode : byte
        {
            Send = 1, Receive = 2
        }
        // UdpConnection

        // RtpSender
        // RtpReceiver
        // RtcpTransceiver

        public RtpOperationMode OperationMode { get; }
        public uint Ssrc { get; }
        public SdesData SourceDescription { get; }

        public RtpSession(uint ssrc, RtpOperationMode operationMode, int rtpPort = 0, int rtcpPort = 0)
        {
            Ssrc = ssrc;
            if (operationMode.HasFlag(RtpOperationMode.Receive))
            {
                if (rtpPort > 0)
                    ;// Start RtpReceiver on specified port.               
                else
                    ;// Start RtpReceiver on arbitrary port.

                // RtpReceiver.Event += ...
            }
            if (rtcpPort > 0)
                ;// Start RtcpTransceiver on specified port. 
            else
                ;// Start RtcpTransceiver on RtpReceiver.Port + 1.

            // RtcpReceiver.Event += ...
        }

        public void StartSending(IPAddress ip, int port)
        {
            // RtpSender.PayloadSource = ...;
            // RtpSender.Start(ip, port);
        }
    }
}