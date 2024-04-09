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
        //public Conversation nextConversation;
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

#pragma warning disable CS0414 // The field '_Element.name' is assigned but its value is never used

    [Serializable]
    public class DialogueElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "Dialogue";
        public LineID lineID;

        public DialogueElement(LineID lineID)
        {
            this.lineID = lineID;
        }
    }

    [Serializable]
    public class IfElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "If";
        public string condition;

        public IfElement(string condition)
        {
            // Remove all quotes
            this.condition = condition.Trim().Replace("\"", string.Empty);
        }
    }

    [Serializable]
    public class ElifElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "Elif";
        public string condition;

        public ElifElement(string condition)
        {
            // Remove all quotes
            this.condition = condition.Trim().Replace("\"", string.Empty);
        }
    }

    [Serializable]
    public class ElseElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "Else";
    }

    [Serializable]
    public class EndIfElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "EndIf";
    }

    [Serializable]
    public class GotoElement : IConversationElement
    {
        [HideInInspector, SerializeField]
        string name = "Goto";
        public Conversation conversation;

        [HideInInspector]
        public string goofyWorkaroundConversationName;

        public GotoElement(string conversationName)
        {
            goofyWorkaroundConversationName = conversationName;
        }

        public GotoElement(Conversation conversation)
        {
            this.conversation = conversation;
        }
    }


#pragma warning restore CS0414 // The field '_Element.name' is assigned but its value is never used

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
