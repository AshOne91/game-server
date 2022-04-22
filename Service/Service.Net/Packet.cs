using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Service.Net
{
    public class Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color()
        {
            R = G = B = A = 255;
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(string HexString)
        {
            StringToColor(HexString);
        }

        public bool StringToColor(string HexString)
        {
            if (HexString.Length != 8)
            {
                return false;
            }

            R = (byte)int.Parse(HexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            G = (byte)int.Parse(HexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            B = (byte)int.Parse(HexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            A = (byte)int.Parse(HexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return true;
        }
    }

    [Serializable()]
    public class PacketException : System.Exception
    {
        public PacketException() : base() { }
        public PacketException(string message) : base(message) { }
        public PacketException(string message, System.Exception inner) : base(message, inner) { }
        protected PacketException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
    public interface IPacketSerializable
    {
        void Serialize(Packet packet);
        void Deserialize(Packet packet);
    }

    public sealed class Packet : IDisposable
    {
        public static readonly ushort PACKETHEADERSIZE = 5;
        public static readonly ushort PACKETBUFFERSIZE = 65535;

        public Packet()
        {
            Clear();
        }

        public Packet(ushort id)
        {
            Clear();
            SetId(id);
        }
        ~Packet()
        {
            this.Dispose(false);
        }


        #region IDisposable Members
        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            try
            {
                if (disposing)
                {
                    _buffer.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                this.disposed = true;
            }



            this.disposed = true;
        }
        #endregion


        public void Clear(int nBufferSize = 8192)
        {
            _receivedSize = 0;
            _readPosition = 0;
            _writePosition = 0;

            _buffer = new MemoryStream(nBufferSize);
            // datafield size 0 으로 세팅
            ushort size = 0;
            _buffer.Write(BitConverter.GetBytes(size), 0, sizeof(ushort));

        }
        public ushort GetId()
        {
            ushort id = BitConverter.ToUInt16(_buffer.GetBuffer(), sizeof(ushort));
            return id;
        }
        public byte GetSeq()
        {
            _buffer.Seek(sizeof(ushort) + sizeof(ushort), SeekOrigin.Begin);
            byte id = Convert.ToByte(_buffer.ReadByte());
            return id;
        }
        public void SetSeq(byte cSeq)
        {
            // seq 쓸 위치로 이동..
            _buffer.Seek(sizeof(ushort) + sizeof(ushort), SeekOrigin.Begin);
            // 
            _buffer.WriteByte(cSeq);

        }
        public void SetId(ushort id)
        {
            // 프로토콜은 3~4번째 인덱스, 
            _buffer.Seek(sizeof(ushort), SeekOrigin.Begin);
            _buffer.Write(BitConverter.GetBytes(id), 0, sizeof(ushort));

        }
        public bool IsValidHeader()
        {
            if (GetPacketSize() >= PACKETHEADERSIZE)
                return true;
            return false;
        }

        public bool IsValidPacket()
        {
            if (IsValidHeader() == false || _receivedSize < PACKETHEADERSIZE || _receivedSize < GetPacketSize())
                return false;

            return true;
        }

        public ushort GetDataFieldSize()
        {
            ushort size = BitConverter.ToUInt16(_buffer.GetBuffer(), 0);
            return size;
        }

        public ushort GetPacketSize()
        {
            ushort nTotalSize = (ushort)(GetDataFieldSize() + PACKETHEADERSIZE);
            return nTotalSize;
        }
        public int GetReceivedSize() { return _receivedSize; }


        public void CopyToBuffer(byte[] data, int nSize)
        {
            if (nSize > PACKETBUFFERSIZE)
            {
                throw new PacketException("NwPacket CopyToBuffer out of range");
            }

            Clear();
            // 처음으로 이동 
            _buffer.Seek(0, SeekOrigin.Begin);
            _buffer.Write(data, 0, nSize);
            _receivedSize += nSize;
        }

        public byte[] GetPacketBuffer()
        {
            return _buffer.GetBuffer();
        }

        public void WriteData(byte[] data, int size)
        {
            if (_writePosition + size > PACKETBUFFERSIZE)
            {
                throw new PacketException("NwPacket WriteData out of range");
            }

            ushort nPrevSize = GetDataFieldSize();

            ushort nTotalSize = (ushort)(nPrevSize + size);

            _buffer.Seek(0, SeekOrigin.Begin);
            _buffer.Write(BitConverter.GetBytes(nTotalSize), 0, sizeof(ushort));
            _buffer.Seek(PACKETHEADERSIZE + _writePosition, SeekOrigin.Begin);
            _buffer.Write(data, 0, size);

            _writePosition += size;
        }

        public void ReadData(ref byte[] data, int size)
        {
            if (_readPosition + size > PACKETBUFFERSIZE)
            {
                throw new PacketException("NwPacket ReadData out of range");
            }

            _buffer.Seek(PACKETHEADERSIZE + _readPosition, SeekOrigin.Begin);
            _buffer.Read(data, 0, size);
            _readPosition += size;
        }


        public void Encrypt(byte cKey)
        {
            SetSeq(cKey);
            byte[] data = _buffer.ToArray();
            PacketCrypt.Encrypt(ref data, PACKETHEADERSIZE, GetPacketSize(), PacketCrypt.GetKey(cKey));
            _buffer.Seek(0, SeekOrigin.Begin);
            _buffer.Write(data, 0, data.Length);

        }

        public void Decrypt()
        {
            byte cKey = GetSeq();

            byte[] data = _buffer.ToArray();
            PacketCrypt.Decrypt(ref data, PACKETHEADERSIZE, GetPacketSize(), PacketCrypt.GetKey(cKey));
            _buffer.Seek(0, SeekOrigin.Begin);
            _buffer.Write(data, 0, data.Length);
        }

        #region Read/Write Overloading Functions
        public void Read(ref int arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(int));
            arg = BitConverter.ToInt32(argData, 0);
        }
        public void Read(ref UInt32 arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(UInt32));
            arg = BitConverter.ToUInt32(argData, 0);
        }
        public void Read(ref byte arg)
        {
            byte[] argData = new byte[1];
            ReadData(ref argData, sizeof(byte));
            arg = argData[0];

        }
        public void Read(ref ulong arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(ulong));
            arg = BitConverter.ToUInt64(argData, 0);
        }
        public void Read(ref ushort arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(ushort));
            arg = BitConverter.ToUInt16(argData, 0);
        }
        public void Read(ref short arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(short));
            arg = BitConverter.ToInt16(argData, 0);
        }
        public void Read(ref double arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(double));
            arg = BitConverter.ToDouble(argData, 0);
        }
        public void Read(ref bool arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(bool));
            arg = BitConverter.ToBoolean(argData, 0);
        }
        public void Read(ref char arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(char));
            arg = BitConverter.ToChar(argData, 0);
        }
        public void Read(ref sbyte arg)
        {
            byte[] argData = new byte[1];
            ReadData(ref argData, sizeof(byte));
            arg = (sbyte)argData[0];
        }
        public void Read(ref Int64 arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(Int64));
            arg = BitConverter.ToInt64(argData, 0);
        }
        public void Read(ref float arg)
        {

            Byte[] argData = BitConverter.GetBytes(arg);
            ReadData(ref argData, sizeof(float));
            arg = BitConverter.ToSingle(argData, 0);
        }

        public void Read(ref String arg)
        {
            int nLength = 0;
            Read(ref nLength);

            Byte[] argData = new Byte[nLength];

            ReadData(ref argData, nLength);
            arg = System.Text.Encoding.UTF8.GetString(argData);
        }
        public void Read(ref DateTime arg)
        {
            Int64 _UtcTicks = 0;
            Read(ref _UtcTicks);
            arg = new DateTime(_UtcTicks, DateTimeKind.Utc).ToLocalTime();
        }
        public void Read(ref Vector3 arg)
        {
            Read(ref arg.X);
            Read(ref arg.Y);
            Read(ref arg.Z);
        }
        public void Read(ref Vector2 arg)
        {
            Read(ref arg.X);
            Read(ref arg.Y);
        }
        public void Read(ref Color arg)
        {
            Byte[] colorArgs = new Byte[4];
            Read(ref colorArgs[0]);
            Read(ref colorArgs[1]);
            Read(ref colorArgs[2]);
            Read(ref colorArgs[3]);

            arg = new Color(colorArgs[0], colorArgs[1], colorArgs[2], colorArgs[3]);
        }
        public void Read(ref byte[] arg)
        {
            int nLength = 0;
            Read(ref nLength);
            ReadData(ref arg, nLength);
        }
        public void Read(ref IPacketSerializable arg)
        {
            arg.Deserialize(this);
        }

        public void Write(String arg)
        {
            byte[] argData = System.Text.Encoding.UTF8.GetBytes(arg);
            int length = argData.Length;
            Write(length);
            WriteData(argData, length);
        }
        public void Write(int arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(int));
        }
        public void Write(UInt32 arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(UInt32));
        }
        public void Write(byte arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(byte));
        }
        public void Write(sbyte arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(sbyte));
        }
        public void Write(ulong arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(ulong));
        }
        public void Write(ushort arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(ushort));
        }
        public void Write(short arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(short));
        }
        public void Write(double arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(double));
        }
        public void Write(bool arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(bool));
        }
        public void Write(char arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(char));
        }
        public void Write(Int64 arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(Int64));
        }
        public void Write(float arg)
        {
            Byte[] argData = BitConverter.GetBytes(arg);
            WriteData(argData, sizeof(float));
        }
        public void Write(DateTime arg)
        {
            Write(arg.ToUniversalTime().Ticks);
        }
        public void Write(Vector3 arg)
        {
            Write(arg.X);
            Write(arg.Y);
            Write(arg.Z);
        }
        public void Write(Vector2 arg)
        {
            Write(arg.X);
            Write(arg.Y);
        }
        public void Write(Color arg)
        {
            Write(arg.R);
            Write(arg.G);
            Write(arg.B);
            Write(arg.A);
        }
        public void Write(IPacketSerializable arg)
        {
            arg.Serialize(this);
        }
        public void Write(byte[] arg, Int32 argSize)
        {
            Write(argSize);
            WriteData(arg, argSize);
        }

        #endregion

        private MemoryStream _buffer;
        private int _receivedSize;
        private int _readPosition;
        private int _writePosition;

    }
}
