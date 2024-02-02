using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToBOE.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject
    {
        /*
        
        Here's the plan kiddo:
        - You create one of these ScriptableObjects in unity
        - It has a list of LineIDs that you assign, with some custom editor to preview the text
        - It has a field for a ChoiceCollection (or a list of choices) that end the convo
        - Convo just ends if there are no choices.

        */

        public List<LineID> lines;
        public List<ConversationChoice> endChoices;

        [System.Serializable]
        public class ConversationChoice
        {
            public LineID prompt;
            public Conversation nextConversation;
            public ChoiceChosenUnityEvent onChosen;
        }

        [System.Serializable]
        public class ChoiceChosenUnityEvent : UnityEvent<LineID> { }
    }
}
