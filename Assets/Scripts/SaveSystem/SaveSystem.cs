using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "Saves");
        private static readonly string SaveFileNameTemplate = "save_#.dat";
        private static readonly string LastPlayedFile = Path.Combine(SavePath, "lastplayed.txt");

        static readonly FileVersion Version = FileVersion.Version_1_0;


        /// <summary>
        /// Saves the given <paramref name="state"/> to the specified <paramref name="saveSlot"/>.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="saveSlot"></param>
        public static void Save(SaveState state, uint saveSlot)
        {
            ByteBuffer buf = new ByteBuffer();
            buf.Add((byte)Version);
            state.AddDataTo(buf);
            SaveBuffer(buf, saveSlot);
        }

        /// <summary>
        /// Loads the save if it exists, or saves and loads an empty save.
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <returns></returns>
        public static SaveState Load(uint saveSlot)
        {
            // Don't let us try to load a non-existent save file
            if (!SaveExists(saveSlot))
            {
                SaveState emptyState = new SaveState();
                // TODO: Support tutorial mode
                emptyState.SaveEmptyState(true);
                // Save this slot so we can use it later
                Save(emptyState, saveSlot);
                // Load the file as normal now
                //return emptyState;
            }

            ByteBuffer buf = LoadBuffer(saveSlot);
            FileVersion saveVersion = (FileVersion)buf.Read<byte>();
            if (saveVersion != Version)
            {
                // idk do something here if the file version ever changes
            }

            SaveState state = new SaveState(buf);
            return state;
        }

        /// <summary>
        /// Deletes the save in the specified <paramref name="saveSlot"/> if it exists.
        /// </summary>
        /// <param name="saveSlot"></param>
        public static void Delete(uint saveSlot)
        {
            if (SaveExists(saveSlot))
            {
                File.Delete(FormatSavePath(saveSlot));
            }
        }

        /// <summary>
        /// Returns true if a file for save number <paramref name="saveSlot"/> exists.
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <returns></returns>
        public static bool SaveExists(uint saveSlot)
        {
            string path = FormatSavePath(saveSlot);
            // Check if it exists
            return File.Exists(path);
        }


        /// <summary>
        /// Attempts to read the slot of the last played save.
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <returns>True if there is a last played save.</returns>
        public static bool TryGetLastPlayedSaveSlot(out uint saveSlot)
        {
            saveSlot = 0;
            return File.Exists(LastPlayedFile) &&
                uint.TryParse(File.ReadAllText(LastPlayedFile), out saveSlot) &&
                SaveExists(saveSlot);
        }

        /// <summary>
        /// Saves the last played save slot to disk.
        /// </summary>
        /// <param name="saveSlot"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void SaveLastPlayedSaveSlot(uint saveSlot)
        {
            if (!SaveExists(saveSlot))
                throw new ArgumentException($"Last played save slot ({saveSlot}) does not exist!", nameof(saveSlot));

            File.WriteAllText(LastPlayedFile, saveSlot.ToString());
        }


        static void SaveBuffer(ByteBuffer buf, uint saveNumber)
        {
            // Make sure our folder exists
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            string path = FormatSavePath(saveNumber);
            try
            {
                // Copy the correct amount of bytes
                byte[] writeBuf = new byte[buf.Written];
                Buffer.BlockCopy(buf.Data, 0, writeBuf, 0, buf.Written);
                // Save them
                File.WriteAllBytes(path, writeBuf);
            }
            catch (System.Exception ex)
            {
                throw new SaveSystemException($"Failed to write save file {saveNumber}: {ex}");
            }
        }

        static ByteBuffer LoadBuffer(uint saveSlot)
        {
            // Make sure we are loading a valid save
            if (!SaveExists(saveSlot))
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
        static string FormatSavePath(uint saveSlot)
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
