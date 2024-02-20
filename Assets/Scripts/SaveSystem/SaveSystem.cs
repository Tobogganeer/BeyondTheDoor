using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveSystem
    {
        public static readonly string SavePath = Path.Combine(Application.persistentDataPath, "Saves");
        public static readonly string SaveFileNameTemplate = "save_#.dat";

        public static void Save()
        {

        }

        public static void Load()
        {

        }

        static void SaveBuffer(ByteBuffer buf, int saveNumber)
        {
            string path = FormatSavePath(saveNumber);
            try
            {
                byte[] writeBuf = new byte[buf.Written];
                System.Buffer.BlockCopy(buf.Data, 0, writeBuf, 0, buf.Written);
                File.WriteAllBytes(path, writeBuf);
            }
            catch (System.Exception ex)
            {
                throw new SaveSystemException($"Failed to write save file {saveNumber}: {ex}");
            }
        }

        static ByteBuffer LoadBuffer(int saveNumber)
        {
            string path = FormatSavePath(saveNumber);

            if (!File.Exists(path))
                throw new SaveSystemException($"Save file {saveNumber} not found.");

            ByteBuffer buf = new ByteBuffer();
            try
            {
                buf.AddByteArray(File.ReadAllBytes(path));
            }
            catch (System.Exception ex)
            {
                throw new SaveSystemException($"Failed to read save file {saveNumber}: {ex}");
            }

            return buf;
        }

        /// <summary>
        /// Returns the full path to save file number <paramref name="saveNumber"/>.
        /// </summary>
        /// <param name="saveNumber">The save file number to format.</param>
        /// <returns></returns>
        static string FormatSavePath(int saveNumber)
        {
            return Path.Combine(SavePath, SaveFileNameTemplate.Replace("#", saveNumber.ToString()));
        }
    }

    public class SaveSystemException : System.Exception
    {
        public SaveSystemException() { }
        public SaveSystemException(string message) : base(message) { }
    }
}
