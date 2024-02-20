using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveState
    {
        private ByteBuffer savedData;

        /// <summary>
        /// Creates an empty SaveState ready for saving.
        /// </summary>
        public SaveState()
        {
            savedData = new ByteBuffer();
            SaveEmptyState();
        }

        /// <summary>
        /// Creates a SaveState with save data ready for loading.
        /// </summary>
        /// <param name="savedData"></param>
        public SaveState(ByteBuffer savedData)
        {
            this.savedData = savedData;
        }

        public void AddDataTo(ByteBuffer buffer)
        {
            buffer.AddBuffer(savedData);
        }

        public void SaveEmptyState()
        {
            SaveEmptyCabin(savedData);
            SaveEmptyDays(savedData);
            SaveEmptyCharacters(savedData);
            SaveEmptyLines(savedData);
        }

        public void SaveCurrentState()
        {
            SaveCabin(savedData);
            SaveDays(savedData);
            SaveCharacters(savedData);
            SaveLines(savedData);
        }

        public void Load()
        {
            // Load everything in the same order
            LoadCabin(savedData);
            LoadDays(savedData);
            LoadCharacters(savedData);
            LoadLines(savedData);
        }

        #region Empty Saving
        private static void SaveEmptyCabin(ByteBuffer buf)
        {

        }

        private static void SaveEmptyDays(ByteBuffer buf)
        {

        }

        private static void SaveEmptyCharacters(ByteBuffer buf)
        {

        }

        private static void SaveEmptyLines(ByteBuffer buf)
        {

        }
        #endregion

        #region Saving
        private static void SaveCabin(ByteBuffer buf)
        {

        }

        private static void SaveDays(ByteBuffer buf)
        {

        }

        private static void SaveCharacters(ByteBuffer buf)
        {

        }

        private static void SaveLines(ByteBuffer buf)
        {

        }
        #endregion

        #region Loading
        private static void LoadCabin(ByteBuffer buf)
        {

        }

        private static void LoadDays(ByteBuffer buf)
        {

        }

        private static void LoadCharacters(ByteBuffer buf)
        {

        }

        private static void LoadLines(ByteBuffer buf)
        {

        }
        #endregion

        private static void AddCharacter(ByteBuffer buf, Character c)
        {

        }

        private static void LoadCharacter(ByteBuffer buf, Character loadInto)
        {

        }

        private static void AddLine(ByteBuffer buf, Line l)
        {

        }

        private static void LoadLine(ByteBuffer buf, Line loadInto)
        {

        }
    }
}
