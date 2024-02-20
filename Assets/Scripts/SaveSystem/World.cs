using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.SaveSystem
{
    public class World : IBufferStruct
    {
        public int SaveSlot { get; private set; }

        // Used by IBufferStruct
        public World() { }

        public static World CreateEmpty()
        {

        }

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
