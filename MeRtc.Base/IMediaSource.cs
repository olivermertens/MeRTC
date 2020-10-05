using System;

namespace MeRtc.Base
{
    public class MediaDescription
    {
        public enum MediaType { Audio, Video }
        public string Name { get; }
        public MediaType Type { get; }
        public string Codec { get; }
        public int SampleRate { get; }
    }
    public interface IMediaSource
    {
        event EventHandler<byte> FrameReady;
    }
}