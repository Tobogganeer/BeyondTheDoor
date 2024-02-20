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
            throw new System.NotImplementedException();
        }

        public void Deserialize(ByteBuffer buf)
        {
            throw new System.NotImplementedException();
        }
    }
}
