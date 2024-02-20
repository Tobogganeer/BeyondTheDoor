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

        /// <summary>
        /// Returns true if a file for save number <paramref name="saveNumber"/> exists.
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public static bool SaveExists(int saveNumber)
        {
            string path = FormatSavePath(saveNumber);
            // Check if it exists
            return File.Exists(path);
        }

        static void SaveBuffer(ByteBuffer buf, int saveNumber)
        {
            // Make sure our folder exists
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            string path = FormatSavePath(saveNumber);
            try
            {
                // Copy the correct amount of bytes
                byte[] writeBuf = new byte[buf.Written];
                System.Buffer.BlockCopy(buf.Data, 0, writeBuf, 0, buf.Written);
                // Save them
                File.WriteAllBytes(path, writeBuf);
            }
            catch (System.Exception ex)
            {
                throw new SaveSystemException($"Failed to write save file {saveNumber}: {ex}");
            }
        }

        static ByteBuffer LoadBuffer(int saveNumber)
        {
            // Make sure we are loading a valid save
            if (SaveExists(saveNumber))
                throw new SaveSystemException($"Save file {saveNumber} not found.");

            string path = FormatSavePath(saveNumber);

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
