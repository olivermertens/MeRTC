using System;
using System.Collections.Generic;

namespace MeRtc.Base
{
    // https://tools.ietf.org/html/rfc3550#section-6.5
    public class SdesData : IRtcpData
    {
        System.Text.UTF8Encoding utf8Encoder = new System.Text.UTF8Encoding();

        public enum SdesItemType : byte
        {
            CName = 1, Name = 2, Email = 3, Phone = 4, Loc = 5, Tool = 6, Note = 7, PrivateExtensions = 8
        }

        public HashSet<SdesItemType> ItemsToWrite { get; set; }
        public HashSet<string> PrivateExtensionsToWrite { get; set; }


        public uint Src { get; set; }

        byte[] cName;
        string CName
        {
            get { if (cName != null) { return utf8Encoder.GetString(cName, 0, cName.Length); } else { return null; } }
            set { if (value != null) cName = utf8Encoder.GetBytes(value); }
        }

        byte[] name;
        string Name
        {
            get { if (name != null) { return utf8Encoder.GetString(name, 0, name.Length); } else { return null; } }
            set { if (value != null) name = utf8Encoder.GetBytes(value); }
        }

        byte[] email;
        string Email
        {
            get { if (email != null) { return utf8Encoder.GetString(email, 0, email.Length); } else { return null; } }
            set { if (value != null) email = utf8Encoder.GetBytes(value); }
        }

        byte[] phone;
        string Phone
        {
            get { if (phone != null) { return utf8Encoder.GetString(phone, 0, phone.Length); } else { return null; } }
            set { if (value != null) phone = utf8Encoder.GetBytes(value); }
        }

        byte[] loc;
        string Loc
        {
            get { if (loc != null) { return utf8Encoder.GetString(loc, 0, loc.Length); } else { return null; } }
            set { if (value != null) loc = utf8Encoder.GetBytes(value); }
        }

        byte[] tool;
        string Tool
        {
            get { if (tool != null) { return utf8Encoder.GetString(tool, 0, tool.Length); } else { return null; } }
            set { if (value != null) tool = utf8Encoder.GetBytes(value); }
        }

        byte[] note;
        string Note
        {
            get { if (note != null) { return utf8Encoder.GetString(note, 0, note.Length); } else { return null; } }
            set { if (value != null) note = utf8Encoder.GetBytes(value); }
        }

        public Dictionary<string, byte[]> PrivateExtensions { get; } = new Dictionary<string, byte[]>();

        public int Size
        {
            get
            {
                int size = 4;
                foreach (var item in ItemsToWrite)
                {
                    if (ItemsToWrite.Contains(item))
                    {
                        switch (item)
                        {
                            case SdesItemType.CName: if (cName != null) { size += 2 + cName.Length; } break;
                            case SdesItemType.Name: if (name != null) { size += 2 + name.Length; } break;
                            case SdesItemType.Email: if (email != null) { size += 2 + email.Length; } break;
                            case SdesItemType.Phone: if (phone != null) { size += 2 + phone.Length; } break;
                            case SdesItemType.Loc: if (loc != null) { size += 2 + loc.Length; } break;
                            case SdesItemType.Tool: if (tool != null) { size += 2 + tool.Length; } break;
                            case SdesItemType.Note: if (note != null) { size += 2 + note.Length; } break;
                            case SdesItemType.PrivateExtensions:
                                foreach (var prvExt in PrivateExtensions)
                                {
                                    if (PrivateExtensionsToWrite.Contains(prvExt.Key))
                                        size += 2 + prvExt.Value.Length;
                                }
                                break;
                        }
                    }
                }
                return size;
            }
        }

        public SdesData()
        {
            ItemsToWrite = new HashSet<SdesItemType>();
            PrivateExtensionsToWrite = new HashSet<string>();
        }

        public void ReadFromBuffer(byte[] buffer, int startIndex)
        {
            Src = BufferConverter.ToUInt32(buffer, startIndex, true);
            int offset = startIndex + 4;
            while (offset < buffer.Length)
            {
                byte[] value = new byte[buffer[offset + 1]];
                Buffer.BlockCopy(buffer, offset + 2, value, 0, value.Length);
                switch ((SdesItemType)buffer[offset])
                {
                    case SdesItemType.CName: cName = value; break;
                    case SdesItemType.Name: name = value; break;
                    case SdesItemType.Email: email = value; break;
                    case SdesItemType.Phone: phone = value; break;
                    case SdesItemType.Loc: loc = value; break;
                    case SdesItemType.Tool: tool = value; break;
                    case SdesItemType.Note: note = value; break;
                    case SdesItemType.PrivateExtensions:
                        byte prefixLength = value[0];
                        string extPrefix = utf8Encoder.GetString(value, 1, prefixLength);
                        byte[] extValue = new byte[value.Length - prefixLength - 1];
                        Buffer.BlockCopy(value, 1 + prefixLength, extValue, 0, extValue.Length);
                        PrivateExtensions[extPrefix] = extValue;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown SDES Type " + buffer[offset] + " received.");
                        return;
                }
                offset += 2 + value.Length;
            }
        }

        public void WriteToBuffer(byte[] buffer, int startIndex)
        {
            BufferConverter.WriteBytes(Src, buffer, startIndex, true);
            int offset = startIndex + 4;
            foreach (var item in ItemsToWrite)
            {
                if (item == SdesItemType.PrivateExtensions)
                {
                    foreach (var pvtExt in PrivateExtensionsToWrite)
                    {
                        byte[] prefix = utf8Encoder.GetBytes(pvtExt);
                        byte[] value = PrivateExtensions[pvtExt];
                        buffer[offset] = (byte)SdesItemType.PrivateExtensions;
                        buffer[offset + 1] = (byte)(value.Length + prefix.Length + 1);
                        buffer[offset + 2] = (byte)(prefix.Length);
                        Buffer.BlockCopy(prefix, 0, buffer, offset + 3, prefix.Length);
                        Buffer.BlockCopy(value, 0, buffer, offset + 3 + prefix.Length, value.Length);
                        offset += 3 + prefix.Length + value.Length;
                    }
                }
                else
                {
                    buffer[offset] = (byte)item;
                    byte[] value = null;
                    switch (item)
                    {
                        case SdesItemType.CName: value = cName; break;
                        case SdesItemType.Name: value = name; break;
                        case SdesItemType.Email: value = email; break;
                        case SdesItemType.Phone: value = phone; break;
                        case SdesItemType.Loc: value = loc; break;
                        case SdesItemType.Tool: value = tool; break;
                        case SdesItemType.Note: value = note; break;
                    }

                    if (value == null)
                        throw new Exception("SDES data has no value for " + item.ToString() + ". Provide a value or remove it from " + nameof(ItemsToWrite) + ".");

                    Buffer.BlockCopy(value, 0, buffer, offset + 2, value.Length);
                }
            }
        }
    }
}