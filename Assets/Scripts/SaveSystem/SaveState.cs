using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.SaveSystem
{
    public class SaveState
    {
        private ByteBuffer savedData;

        public SaveState(ByteBuffer savedData)
        {
            this.savedData = savedData;
        }

        public void AddDataTo(ByteBuffer buffer)
        {
            buffer.AddBuffer(savedData);
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
