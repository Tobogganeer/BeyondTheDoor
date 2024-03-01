using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation Callback")]
    public class ConversationCallback : ScriptableObject
    {
        /// <summary>
        /// Called when this is activated. Passes the target conversation and the line of interest.
        /// </summary>
        /// <remarks>LineID will be the prompt line for a choice, or the first line for conversation start.</remarks>
        public event Action<Conversation, LineID> Callback;

        public void Invoke(Conversation convo, LineID prompt)
        {
            Callback?.Invoke(convo, prompt);
        }
    }
}
