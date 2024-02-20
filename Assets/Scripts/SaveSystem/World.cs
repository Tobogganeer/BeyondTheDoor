using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.SaveSystem
{
    public class World : IBufferStruct
    {
        /// <summary>
        /// Creates an empty world - used by IBufferStruct.
        /// </summary>
        public World() { }

        public void Serialize(ByteBuffer buf)
        {
            SaveCabin(buf);
            SaveDays(buf);
            SaveCharacters(buf);
            SaveLines(buf);
        }

        public void Deserialize(ByteBuffer buf)
        {
            // Load everything in the same order
            LoadCabin(buf);
            LoadDays(buf);
            LoadCharacters(buf);
            LoadLines(buf);
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
