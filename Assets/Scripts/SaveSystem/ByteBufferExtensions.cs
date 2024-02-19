using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace BeyondTheDoor.SaveSystem
{
    public static class ByteBufferExtensions
    {
        public static void ReadData(this ByteBuffer buf, IntPtr ptr, int size)
        {
            buf.WritePosition = size;
            buf.ReadPosition = 0;

            int bufSize = buf.Data.Length;
            if (size > bufSize)
            {
                Debug.LogError($"Can't fully handle {size} bytes because it exceeds the maximum of {bufSize}, message will contain incomplete data!");
                Marshal.Copy(ptr, buf.Data, 0, bufSize);
                buf.SetReadable(bufSize);
            }
            else
            {
                Marshal.Copy(ptr, buf.Data, 0, size);
                buf.SetReadable(size);
            }
        }

        public static void ReadData(this ByteBuffer buf, byte[] bytes)
        {
            int size = bytes.Length;
            buf.WritePosition = size;
            buf.ReadPosition = 0;

            int bufSize = buf.Data.Length;
            if (size > bufSize)
            {
                Debug.LogError($"Can't fully handle {size} bytes because it exceeds the maximum of {bufSize}, message will contain incomplete data!");
                //Marshal.Copy(bytes, buf.Data, 0, bufSize);
                Buffer.BlockCopy(bytes, 0, buf.Data, 0, bufSize);
                buf.SetReadable(bufSize);
            }
            else
            {
                Buffer.BlockCopy(bytes, 0, buf.Data, 0, size);
                buf.SetReadable(size);
            }
        }
    }
}