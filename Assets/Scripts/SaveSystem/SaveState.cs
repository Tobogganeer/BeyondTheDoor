using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // TODO: Implement default save state
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

        #region Saving
        private void SaveCabin(ByteBuffer buf)
        {

        }

        private void SaveDays(ByteBuffer buf)
        {

        }

        private void SaveCharacters(ByteBuffer buf)
        {

        }

        private void SaveLines(ByteBuffer buf)
        {

        }
        #endregion

        #region Loading
        private void LoadCabin(ByteBuffer buf)
        {

        }

        private void LoadDays(ByteBuffer buf)
        {

        }

        private void LoadCharacters(ByteBuffer buf)
        {

        }

        private void LoadLines(ByteBuffer buf)
        {

        }
        #endregion
    }
}
