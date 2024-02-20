using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation Callback")]
    public class ConversationCallback : ScriptableObject
    {
        public event Action<LineID> Chosen;

        internal void OnChosen(LineID prompt)
        {
            Chosen?.Invoke(prompt);
        }
    }
}
