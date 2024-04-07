using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject, IDialogueElement
    {
        // TODO: Add ConversationCallbacks to each line? May not be needed due to Line.OnOpen, but still
        [SerializeField] internal List<LineID> lines;
        [SerializeField] internal Conversation nextConversation;
        [SerializeField] internal List<ConversationChoice> choices;

        //private List<Line> _linesBacking;
        private ChoiceCollection _choicesBacking;

        /// <summary>
        /// The lines of this conversation, in order.
        /// </summary>
        public List<Line> Lines => lines != null ? lines.ConvertAll((id) => Line.Get(id)) : new List<Line>();

        /// <summary>
        /// The choices that can be chosen after all lines are spoken. May be null if the conversation simply ends.
        /// </summary>
        public ChoiceCollection Choices
        {
            get
            {
                // No choices, return nothing
                if (choices.Count == 0)
                    return null;

                if (_choicesBacking == null || _choicesBacking.Choices.Count != choices.Count)
                {
                    List<Choice> fill = new List<Choice>();

                    foreach (ConversationChoice c in choices)
                    {
                        fill.Add(Choice.LineAndAction(Line.Get(c.prompt),
                            c.nextConversation, (line) => c.callback?.Invoke(this, line.ID)));
                    }

                    _choicesBacking = new ChoiceCollection(fill);
                }

                return _choicesBacking;
            }
        }
        /// <summary>
        /// Called when this conversation is started
        /// </summary>
        public ConversationCallback OnStarted;
        /// <summary>
        /// Called when the last line of this conversation is finished, or when a choice is chosen.
        /// </summary>
        public ConversationCallback OnFinished;


        /// <summary>
        /// Starts this conversation. Analogous to Open();
        /// </summary>
        public void Start()
        {
            Open();
        }

        public void Open()
        {
            HookUpLines();
            List<Line> lines = Lines;
            OnStarted?.Invoke(this, lines.Count > 0 ? lines[0].ID : 0);
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
        }

        private void HookUpLines()
        {
            List<Line> _linesBacking = Lines;

            // Make the lines lead into each other
            if (_linesBacking.Count > 1)
            {
                for (int i = 0; i < _linesBacking.Count - 1; i++)
                    _linesBacking[i].Then(_linesBacking[i + 1]);
                // Make the last one lead to the choices/next convo (can't use ?? here)
                _linesBacking[_linesBacking.Count - 1].Then(nextConversation == null ? Choices : nextConversation);
            }
        }

        private void HookUpLineFinishedCallback(Line line)
        {
            // Un and re-sub to prevent double calling
            line.OnClose -= OnLastLineFinished;
            line.OnClose += OnLastLineFinished;
        }

        private void HookUpLineFinishedCallback(Choice line)
        {
            // Un and re-sub to prevent double calling
            line.OnChosen -= OnLastLineFinished;
            line.OnChosen += OnLastLineFinished;
        }

        private void OnLastLineFinished(Line line)
        {
            OnFinished?.Invoke(this, line.ID);
        }


        [Serializable]
        internal class ConversationChoice
        {
            public LineID prompt;
            public Conversation nextConversation;
            public ConversationCallback callback;
        }
    }
}
