using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using ArrayLength = System.UInt16;

namespace BeyondTheDoor.SaveSystem
{
    public class ByteBuffer
    {
        const ushort BufferSize = 1024 * 8; // 8kb should be enough for these saves

        public readonly byte[] Data;

        public int WritePosition = 0;
        public int ReadPosition = 0;
        public int Readable { get; private set; }
        public int Unread => Readable - ReadPosition;
        public int Unwritten => Data.Length - WritePosition;
        public int Written => WritePosition;

        public ByteBuffer(ushort maxSize = BufferSize)
        {
            Data = new byte[maxSize];
        }

        public void Reset()
        {
            WritePosition = 0;
            ReadPosition = 0;
        }

        internal void SetReadable(int readable)
        {
            Readable = readable;
        }

        #region Byte
        public ByteBuffer AddByte(byte value)
        {
            if (Unwritten < 1)
                throw new Exception($"Failed to write byte ({Unwritten} bytes unwritten)");

            Data[WritePosition++] = value;
            Readable++;
            return this;
        }

        public byte GetByte()
        {
            if (Unread < 1)
            {
                Debug.LogError($"Failed to read byte ({Unread} bytes unread)");
                return 0;
            }

            return Data[ReadPosition++];
        }

        public byte PeekByte()
        {
            if (Unread < 1)
            {
                Debug.LogError($"Failed to peek byte ({Unread} bytes unread)");
                return 0;
            }

            return Data[ReadPosition];
        }

        public ByteBuffer AddByteArray(byte[] value)
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException("value");

            if (Unwritten < value.Length)
                throw new Exception($"Failed to write byte[{value.Length}] ({Unwritten} bytes unwritten)");

            Array.Copy(value, 0, Data, WritePosition, value.Length);
            WritePosition += (ushort)value.Length;
            Readable += value.Length;
            return this;
        }

        public byte[] GetByteArray(int length)
        {
            byte[] value = new byte[length];

            if (Unread < length)
            {
                Debug.LogError($"Failed to read byte[{length}] ({Unread} bytes unread)");
                length = Unread;
            }

            Array.Copy(Data, ReadPosition, value, 0, length);
            ReadPosition += (ushort)length;
            return value;
        }
        #endregion

        #region Span

        public ByteBuffer WriteSpan(ReadOnlySpan<byte> span)
        {
            if (Unwritten < span.Length)
                throw new Exception($"Failed to write Span ({Unwritten} bytes unwritten)");

            Add((ushort)span.Length);
            span.CopyTo(new Span<byte>(Data, WritePosition, span.Length));
            WritePosition += (ushort)span.Length;
            Readable += span.Length;
            return this;
        }

        public unsafe ByteBuffer WriteSpan<T>(ReadOnlySpan<T> span) where T : unmanaged
        {
            Add((ushort)span.Length);

            if (Unwritten < span.Length * sizeof(T))
                throw new Exception($"Failed to write Span<{typeof(T)}> ({Unwritten} bytes unwritten)");

            Span<byte> bytes = new Span<byte>(Data, WritePosition, sizeof(T) * span.Length);

            for (int i = 0; i < span.Length; i++)
            {
                T t = span[i];
                MemoryMarshal.Write(bytes, ref t);
            }

            WritePosition += (ushort)(span.Length * sizeof(T));
            Readable += span.Length * sizeof(T);
            return this;
        }

        #endregion

        #region String
        public ByteBuffer AddString(string value)
        {
            // Generates garbage, idc rn its 3:58 am
            if (value == null || value.Length == 0)
            {
                Add((ushort)0);
                return this;
            }

            //Add(value.ToCharArray());
            SetString(WritePosition, value);

            WritePosition += value.Length * sizeof(char) + sizeof(ushort);

            return this;
        }

        unsafe void SetString(int start, string value)
        {
            Span<byte> sizeDestinationSpan = new Span<byte>(Data, start, sizeof(ushort));
            ushort strSize = (ushort)value.Length;
            MemoryMarshal.Write(sizeDestinationSpan, ref strSize);

            Span<byte> stringDestinationSpan = new Span<byte>(Data, start + sizeof(ushort), value.Length * sizeof(char));

            ReadOnlySpan<byte> arrayBytes = MemoryMarshal.AsBytes(value.AsSpan());

            for (int i = 0; i < stringDestinationSpan.Length; i++)
            {
                stringDestinationSpan[i] = arrayBytes[i];
            }
        }

        public ByteBuffer AddStringArray(string[] values)
        {
            if (values == null || values.Length == 0)
            {
                Add(0);
                return this;
            }

            Add(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                AddString(values[i]);
            }

            return this;
        }

        public string Read()
        {
            return ReadString();
        }

        public string ReadString()
        {
            if (Peek<ushort>() == 0)
            {
                ReadPosition += sizeof(ushort);
                return "";
            }
            string str = GetString(ReadPosition);
            ReadPosition += str.Length * sizeof(char) + sizeof(ushort);
            return str;
            //return new string(ReadArray<char>());
        }

        unsafe string GetString(int start)
        {
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(Data, start, sizeof(ushort));
            ushort length = MemoryMarshal.Read<ushort>(bytes);

            return string.Create(length, 0, (Span<char> chars, int _) =>
            {
                ReadOnlySpan<byte> stringChars = new ReadOnlySpan<byte>(Data, start + sizeof(ushort), length * sizeof(char));

                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = MemoryMarshal.Read<char>(stringChars.Slice(i * sizeof(char), sizeof(char)));
                }
            });
        }

        public string[] ReadStringArray()
        {
            int len = Read<int>();
            string[] array = new string[len];
            for (int i = 0; i < len; i++)
            {
                array[i] = Read();
            }

            return array;
        }
        #endregion

        public ByteBuffer AddBuffer(ByteBuffer buf)
        {
            Add(buf.Written);
            WriteSpan(new ReadOnlySpan<byte>(buf.Data, 0, buf.Written));
            return this;
        }

        public ByteBuffer GetBuffer()
        {
            int size = Read<int>();
            ByteBuffer read = new ByteBuffer();
            read.WriteSpan(new ReadOnlySpan<byte>(Data, ReadPosition, size));
            return read;
        }

        #region Buffer Struct

        public ByteBuffer WriteStruct<T>(T bufferStruct) where T : IBufferStruct
        {
            bufferStruct.Serialize(this);
            return this;
        }

        public ByteBuffer AddStruct<T>(T bufferStruct) where T : IBufferStruct
        {
            bufferStruct.Serialize(this);
            return this;
        }

        public T GetStruct<T>() where T : IBufferStruct, new()
        {
            //T bufferStruct = default(T);
            T bufferStruct = FastActivator<T>.Create();
            bufferStruct.Deserialize(this);
            return bufferStruct;
        }

        public T ReadStruct<T>() where T : IBufferStruct, new()
        {
            //T bufferStruct = default(T);
            T bufferStruct = FastActivator<T>.Create();
            bufferStruct.Deserialize(this);
            return bufferStruct;
        }

        #endregion

        #region Unmanaged

        public unsafe ByteBuffer Add<T>(T value) where T : unmanaged
        {
            int size = sizeof(T);

            if (Unwritten < size)
                throw new Exception($"Failed to write {typeof(T)} ({Unwritten} bytes unwritten)");

            Span<byte> span = new Span<byte>(Data, WritePosition, size);
            MemoryMarshal.Write(span, ref value);

            WritePosition += (ushort)size;
            Readable += size;
            return this;
        }

        /// <summary>
        /// Writes an array of values to the buffer
        /// </summary>
        /// <typeparam name="T">The unmanaged type to write,</typeparam>
        /// <param name="value">The array of values,</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public unsafe ByteBuffer Add<T>(T[] value) where T : unmanaged
        {
            int size = sizeof(T);

            if (Unwritten < size * value.Length + sizeof(ArrayLength))
                throw new Exception($"Failed to write {typeof(T)}[] ({Unwritten} bytes unwritten)");

            if (value == null)
            {
                Debug.LogWarning($"Tried to write null {typeof(T)}[] to ByteBuffer, writing empty array!");
                WriteArrayLength(0);
                return this;
            }

            WriteArrayLength(value.Length);
            Span<byte> span = new Span<byte>(Data, WritePosition, size * value.Length);

            if (value != null && value.Length > 0)
            {
                Span<byte> arrayBytes = MemoryMarshal.AsBytes(value.AsSpan());

                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = arrayBytes[i];
                }

                WritePosition += (ushort)(size * value.Length);
                Readable += size * value.Length;
            }
            return this;
        }

        private unsafe void WriteArrayLength(int count)
        {
            Add((ArrayLength)count);
        }

        public unsafe T Read<T>() where T : unmanaged
        {
            int size = sizeof(T);

            if (Unread < sizeof(T))
            {
                Debug.LogError($"Failed to read {typeof(T)} ({Unread} bytes unread)");
                return default(T);
            }

            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(Data, ReadPosition, sizeof(T));
            T value = MemoryMarshal.Read<T>(bytes);

            ReadPosition += (ushort)size;
            return value;
        }

        public unsafe ByteBuffer Read<T>(out T val) where T : unmanaged
        {
            val = Read<T>();
            return this;
        }

        public unsafe T Peek<T>() where T : unmanaged
        {
            T value = Read<T>();
            int size = sizeof(T);
            ReadPosition -= (ushort)size;
            return value;
        }


        /// <summary>
        /// Reads an array of elements.
        /// </summary>
        /// <typeparam name="T">The unmanaged type to read.</typeparam>
        /// <returns></returns>
        public unsafe T[] ReadArray<T>() where T : unmanaged
        {
            int size = sizeof(T);
            ArrayLength len = Read<ArrayLength>();

            if (Unread < sizeof(T) * len)
            {
                Debug.LogError($"Failed to read {typeof(T)}[] ({Unread} bytes unread)");
                return null;
            }

            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(Data, ReadPosition, sizeof(T) * len);
            T[] values = new T[len];

            for (int i = 0; i < len; i++)
            {
                ReadOnlySpan<byte> valBytes = bytes.Slice(i * size, size);
                values[i] = MemoryMarshal.Read<T>(valBytes);
            }

            ReadPosition += (ushort)(size * len);
            return values;
        }

        #endregion

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message: Readable({Readable}), Unread({Unread})," +
                $"Written/WritePos({WritePosition}), Unwritten({Unwritten}), ReadPos({ReadPosition})");
            sb.Append("Bytes: ");
            for (int i = 0; i < WritePosition; i++)
            {
                sb.Append(Data[i] + " ");
            }

            sb.Append("(Written Data ends...)");

            return sb.ToString();
        }
    }

    /// <summary>
    /// Used for customizing the behaviour of content added to buffers. Use WriteStruct<![CDATA[<]]>T<![CDATA[>]]>
    /// </summary>
    public interface IBufferStruct
    {
        void Serialize(ByteBuffer buf);

        void Deserialize(ByteBuffer buf);
    }
}