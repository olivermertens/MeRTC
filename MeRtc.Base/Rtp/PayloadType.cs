using System;
using System.Collections.Generic;

namespace MeRtc.Base
{
    public class PayloadType
    {
        [Flags]
        public enum AudioVideo { Audio = 1, Video = 2 }

        public int Number { get; }
        public string Codec { get; }
        public bool Audio { get; }
        public bool Video { get; }
        public int SampleRate { get; }
        public int? AudioChannels { get; }

        private PayloadType(int number, string codec, bool audio, bool video, int sampleRate, int? audioChannels = null)
        {
            Number = number;
            Codec = codec;
            Audio = audio;
            Video = video;
            SampleRate = sampleRate;
            AudioChannels = audioChannels;
        }

        static PayloadType()
        {
            typesDictionary = new Dictionary<int, PayloadType>
                {
                    {0, PCMU}, {3, GSM}, {4, G723}, {5, DVI4_8kHz}, {6, DVI4_16kHz}, {7, LPC}, {8, PCMA}, {9, G722}, {10, L16_Mono}, {11, L16_Stereo}, {12, QCELP}, {13, CN}, {14, MPA}, {15, G728}, {16, DVI4_11kHz}, {17, DVI4_22kHz}, {18, G729}, {25, CelB}, {26, JPEG}, {28, nv}, {31, H261}, {32, MPV}, {33, MP2T}, {34, H263}, {127, Unknown}
                };
        }

        public static PayloadType GetPayloadType(int number)
        {
            if (typesDictionary.TryGetValue(number, out PayloadType value))
                return value;
            else
                return null;
        }

        static Dictionary<int, PayloadType> typesDictionary;
        public static PayloadType PCMU = new PayloadType(0, "PCMU", true, false, 8000, 1);
        public static PayloadType GSM = new PayloadType(3, "GSM", true, false, 8000, 1);
        public static PayloadType G723 = new PayloadType(4, "G723", true, false, 8000, 1);
        public static PayloadType DVI4_8kHz = new PayloadType(5, "DVI4", true, false, 8000, 1);
        public static PayloadType DVI4_16kHz = new PayloadType(6, "DVI4", true, false, 16000, 1);
        public static PayloadType LPC = new PayloadType(7, "LPC", true, false, 8000, 1);
        public static PayloadType PCMA = new PayloadType(8, "PCMA", true, false, 8000, 1);
        public static PayloadType G722 = new PayloadType(9, "G.722", true, false, 8000, 1);
        public static PayloadType L16_Mono = new PayloadType(10, "L16", true, false, 44100, 1);
        public static PayloadType L16_Stereo = new PayloadType(11, "L16", true, false, 44100, 2);
        public static PayloadType QCELP = new PayloadType(12, "QCELP", true, false, 8000, 1);
        public static PayloadType CN = new PayloadType(13, "CN", true, false, 8000, 1);
        public static PayloadType MPA = new PayloadType(14, "MPA", true, false, 90000, 1);
        public static PayloadType G728 = new PayloadType(15, "G.728", true, false, 8000, 1);
        public static PayloadType DVI4_11kHz = new PayloadType(16, "DVI4", true, false, 11025, 1);
        public static PayloadType DVI4_22kHz = new PayloadType(17, "DVI4", true, false, 22050, 1);
        public static PayloadType G729 = new PayloadType(18, "G.729", true, false, 8000, 1);
        public static PayloadType CelB = new PayloadType(25, "CelB", false, true, 90000);
        public static PayloadType JPEG = new PayloadType(26, "JPEG", false, true, 90000);
        public static PayloadType nv = new PayloadType(28, "nv", false, true, 90000);
        public static PayloadType H261 = new PayloadType(31, "H.261", false, true, 90000);
        public static PayloadType MPV = new PayloadType(32, "MPV", false, true, 90000);
        public static PayloadType MP2T = new PayloadType(33, "MP2T", true, true, 90000);
        public static PayloadType H263 = new PayloadType(34, "H.263", false, true, 90000);
        public static PayloadType Unknown = new PayloadType(127, "", false, false, 0);
    }
}