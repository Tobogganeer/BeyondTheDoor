using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject, IDialogueElement
    {
        /// <summary>
        /// Called when this conversation is started
        /// </summary>
        public ConversationCallback onStarted;

        // TODO: Add ConversationCallbacks to each line? May not be needed due to Line.OnOpen, but still
        [SerializeReference]
        public List<IConversationElement> elements;
        public Conversation nextConversation;
        public List<ConversationChoice> choices;

        /// <summary>
        /// Called when the last line of this conversation is finished, or when a choice is chosen.
        /// </summary>
        public ConversationCallback onFinished;

        /// <summary>
        /// Starts this conversation. Analogous to Open();
        /// </summary>
        public void Start()
        {
            Open();
        }

        public void Open()
        {
            UI.DialogueGUI.Open(this);

            /*
            HookUpLines();
            List<Line> lines = Lines;
            onStarted?.Invoke(this, lines.Count > 0 ? lines[0].ID : 0);
            if (lines.Count > 0)
                lines[0].Open();
            else if (Choices != null)
                Choices?.Open();
            else
                // Close if we have nothing to show
                UI.DialogueGUI.Close();

            // Hook up the OnFinished event
            if (Choices != null && Choices.Choices.Count > 0)
            {
                // We are finished when we pick a choice
                foreach (Choice c in Choices.Choices)
                    HookUpLineFinishedCallback(c);
            }
            else if (lines.Count > 0)
            {
                // There are no choices - we are finished when the last line is closed
                HookUpLineFinishedCallback(lines[lines.Count - 1]);
            }
            */
        }
    }

    [Serializable]
    public class ConversationChoice
    {
        public LineID prompt;
        public Conversation nextConversation;
        public ConversationCallback callback;
    }


    #region Conversation Elements
    public interface IConversationElement { }

    [Serializable]
    public class DialogueElement : IConversationElement
    {
        public LineID lineID;
    }

    [Serializable]
    public class ConditionalElement : IConversationElement
    {
        public string condition;
    }

    [Serializable]
    public class IfElement : ConditionalElement { }

    [Serializable]
    public class ElifElement : ConditionalElement { }

    [Serializable]
    public class ElseElement : IConversationElement { }

    [Serializable]
    public class EndIfElement : IConversationElement { }

    [Serializable]
    public class GotoElement : IConversationElement
    {
        public Conversation conversation;
    }

    /*
    Dialogue,
    ConversationStart,
    ConversationEnd,
    If,
    Elif,
    Else,
    EndIf,
    Choice,
    ChoicePrompt,
    Goto,
    */
    #endregion
}
