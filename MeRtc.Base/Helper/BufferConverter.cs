using System;
using System.Runtime.InteropServices;

namespace MeRtc.Base
{
    public static class BufferConverter
    {

#if BIGENDIAN
        public static readonly bool IsLittleEndian /* = false */;
#else
        public static readonly bool IsLittleEndian = true;
#endif

        private static bool CheckBufferSize(byte[] buffer, int offset, int neededSize)
        {
            return (buffer.Length - offset) >= neededSize;
        }

        private static unsafe void ReverseCopyBytes(byte* from, byte* to, int count)
        {
            for (int i = 0; i < count; i++)
                *(to + count - 1 - i) = *(from + i);
        }

        public static unsafe int WriteBytes(short value, byte[] buffer, int offset, bool bigEndian = false)
        {
            if (!CheckBufferSize(buffer, offset, 2))
                throw new ArgumentException("Buffer too small.");

            fixed (byte* b = &buffer[offset])
            {
                if (IsLittleEndian && bigEndian || !IsLittleEndian && !bigEndian)
                {
                    byte* ptr = (byte*)&value;
                    ReverseCopyBytes(ptr, b, 2);
                }
                else
                    *((short*)b) = value;
            }
            return 2;
        }

        public static unsafe int WriteBytes(int value, byte[] buffer, int offset, bool bigEndian = false)
        {
            if (!CheckBufferSize(buffer, offset, 4))
                throw new ArgumentException("Buffer too small.");

            fixed (byte* b = &buffer[offset])
            {
                if (IsLittleEndian && bigEndian || !IsLittleEndian && !bigEndian)
                {
                    byte* ptr = (byte*)&value;
                    ReverseCopyBytes(ptr, b, 4);
                }
                else
                    *((int*)b) = value;
            }
            return 4;
        }

        public static unsafe int WriteBytes(long value, byte[] buffer, int offset, bool bigEndian = false)
        {
            if (!CheckBufferSize(buffer, offset, 8))
                throw new ArgumentException("Buffer too small.");

            fixed (byte* b = &buffer[offset])
            {
                if (IsLittleEndian && bigEndian || !IsLittleEndian && !bigEndian)
                {
                    byte* ptr = (byte*)&value;
                    ReverseCopyBytes(ptr, b, 8);
                }
                else
                    *((long*)b) = value;
            }
            return 8;
        }

        public static int WriteBytes(ushort value, byte[] buffer, int offset, bool bigEndian = false)
        {
            return WriteBytes((short)value, buffer, offset, bigEndian);
        }

        public static int WriteBytes(uint value, byte[] buffer, int offset, bool bigEndian = false)
        {
            return WriteBytes((int)value, buffer, offset, bigEndian);
        }

        public static int WriteBytes(ulong value, byte[] buffer, int offset, bool bigEndian = false)
        {
            return WriteBytes((long)value, buffer, offset, bigEndian);
        }

        public static unsafe int WriteBytes(float value, byte[] buffer, int offset, bool bigEndian = false)
        {
            return WriteBytes(*(int*)&value, buffer, offset, bigEndian);
        }

        public static unsafe int WriteBytes(double value, byte[] buffer, int offset, bool bigEndian = false)
        {
            return WriteBytes(*(long*)&value, buffer, offset, bigEndian);
        }

        public static unsafe short ToInt16(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            if (buffer == null)
                throw new System.ArgumentNullException(nameof(buffer));

            if (buffer.Length - startIndex < 2)
                throw new ArgumentException("Buffer too small.");



            fixed (byte* pbyte = &buffer[startIndex])
            {
                if (IsLittleEndian && isBigEndian || !IsLittleEndian && !isBigEndian)
                {
                    return (short)((*pbyte << 8) | (*(pbyte + 1)));
                }
                else
                {
                    if ((startIndex & 1) == 0) // data is aligned
                    {
                        return *((short*)pbyte);
                    }
                    else
                    {
                        return (short)((*pbyte) | (*(pbyte + 1) << 8));
                    }
                }
            }
        }

        public static unsafe int ToInt32(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            if (buffer == null)
                throw new System.ArgumentNullException(nameof(buffer));

            if (buffer.Length - startIndex < 4)
                throw new ArgumentException("Buffer too small.");



            fixed (byte* pbyte = &buffer[startIndex])
            {
                if (IsLittleEndian && isBigEndian || !IsLittleEndian && !isBigEndian)
                {
                    return (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | *(pbyte + 3);
                }
                else
                {
                    if (startIndex % 4 == 0) // data is aligned
                    {
                        return *((int*)pbyte);
                    }
                    else
                    {
                        return *pbyte | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                    }
                }
            }
        }

        public static unsafe long ToInt64(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            if (buffer == null)
                throw new System.ArgumentNullException(nameof(buffer));

            if (buffer.Length - startIndex < 4)
                throw new ArgumentException("Buffer too small.");



            fixed (byte* pbyte = &buffer[startIndex])
            {
                if (IsLittleEndian && isBigEndian || !IsLittleEndian && !isBigEndian)
                {
                    var i1 = (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | *(pbyte + 3);
                    int i2 = (*(pbyte + 4) << 24) | (*(pbyte + 5) << 16) | (*(pbyte + 6) << 8) | *(pbyte + 7);
                    return (uint)i2 | (long)i1 << 32;
                }
                else
                {
                    if (startIndex % 8 == 0) // data is aligned
                    {
                        return *((long*)pbyte);
                    }
                    else
                    {
                        var i1 = (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                        int i2 = (*(pbyte + 4)) | (*(pbyte + 5) << 8) | (*(pbyte + 6) << 16) | (*(pbyte + 7) << 24);
                        return (uint)i1 | (long)i2 << 32;
                    }
                }
            }
        }

        public static ushort ToUInt16(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            return (ushort)ToInt16(buffer, startIndex, isBigEndian);
        }

        public static uint ToUInt32(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            return (uint)ToInt32(buffer, startIndex, isBigEndian);
        }

        public static ulong ToUInt64(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            return (ulong)ToInt64(buffer, startIndex, isBigEndian);
        }

        public unsafe static float ToSingle(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            int val = ToInt32(buffer, startIndex, isBigEndian);
            return *(float*)&val;
        }

        public unsafe static double ToDouble(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            long val = ToInt64(buffer, startIndex, isBigEndian);
            return *(double*)&val;
        }

        public unsafe static int ThreeBytesToInt(byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            fixed (byte* pbyte = &buffer[startIndex])
            {
                if (IsLittleEndian && isBigEndian || !IsLittleEndian && !isBigEndian)
                    return *pbyte << 16 | *(pbyte + 1) << 8 | *(pbyte + 2);
                else
                    return *pbyte | *(pbyte + 1) << 8 | *(pbyte + 2) << 16;
            }
        }

        public unsafe static void WriteIntToThreeBytes(int value, byte[] buffer, int startIndex, bool isBigEndian = false)
        {
            fixed (byte* pbyte = &buffer[startIndex])
            {
                byte* ptr = (byte*)&value;
                if (IsLittleEndian && isBigEndian || !IsLittleEndian && !isBigEndian)     
                    ReverseCopyBytes(ptr, pbyte, 3);
                
                else    
                    Buffer.MemoryCopy(ptr, pbyte, 3, 3);      
            }
        }
    }
}