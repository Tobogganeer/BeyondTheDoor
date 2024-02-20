using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveSystem
    {
        public static readonly string SavePath = Path.Combine(Application.persistentDataPath, "Saves");
        public static readonly string SaveFileNameTemplate = "save_#.dat";

        static readonly FileVersion Version = FileVersion.Version_1_0;

        public static void Save(SaveState world, int saveSlot)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Add((byte)Version);
            buf.AddStruct(world);
            SaveBuffer(buf, saveSlot);
        }

        // The world loads all data on deserialization... why are we returning it? Beats me :P
        public static SaveState Load(int saveSlot)
        {
            // Don't let us try to load a non-existent save file
            if (!SaveExists(saveSlot))
            {
                SaveState emptyWorld = new SaveState();
                // Save this slot so we can use it later
                Save(emptyWorld, saveSlot);
                return emptyWorld;
            }

            ByteBuffer buf = LoadBuffer(saveSlot);
            FileVersion saveVersion = (FileVersion)buf.Read<byte>();
            if (saveVersion != Version)
            {
                // idk do something here if the file version ever changes
            }

            SaveState world = buf.GetStruct<SaveState>();
            return world;
        }

        /// <summary>
        /// Returns true if a file for save number <paramref name="saveSlot"/> exists.
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <returns></returns>
        public static bool SaveExists(int saveSlot)
        {
            string path = FormatSavePath(saveSlot);
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

        static ByteBuffer LoadBuffer(int saveSlot)
        {
            // Make sure we are loading a valid save
            if (SaveExists(saveSlot))
                throw new SaveSystemException($"Save file {saveSlot} not found.");

            string path = FormatSavePath(saveSlot);

            ByteBuffer buf = new ByteBuffer();
            try
            {
                buf.AddByteArray(File.ReadAllBytes(path));
            }
            catch (System.Exception ex)
            {
                throw new SaveSystemException($"Failed to read save file {saveSlot}: {ex}");
            }

            return buf;
        }

        /// <summary>
        /// Returns the full path to save file number <paramref name="saveSlot"/>.
        /// </summary>
        /// <param name="saveSlot">The save file number to format.</param>
        /// <returns></returns>
        static string FormatSavePath(int saveSlot)
        {
            return Path.Combine(SavePath, SaveFileNameTemplate.Replace("#", saveSlot.ToString()));
        }

        enum FileVersion : byte
        {
            Unknown,
            Version_1_0
        }
    }

    public class SaveSystemException : Exception
    {
        public SaveSystemException() { }
        public SaveSystemException(string message) : base(message) { }
    }
}
