using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject, IDialogueElement
    {
        [SerializeField] internal List<LineID> lines;
        [SerializeField] internal List<ConversationChoice> choices;

        //private List<Line> _linesBacking;
        private ChoiceCollection _choicesBacking;

        /// <summary>
        /// The lines of this conversation, in order.
        /// </summary>
        public List<Line> Lines => lines?.ConvertAll((id) => Line.Get(id));

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
                        fill.Add(Choice.Line(Line.Get(c.prompt), c.nextConversation));
                    }

                    _choicesBacking = new ChoiceCollection(fill);
                }

                return _choicesBacking;
            }
        }
        /// <summary>
        /// Called when this conversation is started
        /// </summary>
        public event Action<Conversation> OnStarted;



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
            OnStarted?.Invoke(this);
            if (Lines.Count > 0)
                Lines[0].Open();
            else if (Choices != null)
                Choices?.Open();
            else
                // Close if we have nothing to show
                UI.DialogueGUI.Close();
        }

        private void HookUpLines()
        {
            List<Line> _linesBacking = Lines;

            // Make the lines lead into each other
            if (_linesBacking.Count > 1)
            {
                for (int i = 0; i < _linesBacking.Count - 1; i++)
                    _linesBacking[i].Then(_linesBacking[i + 1]);
                // Make the last one lead to the choices
                _linesBacking[_linesBacking.Count - 1].Then(Choices);
            }
        }


        [Serializable]
        internal class ConversationChoice
        {
            public LineID prompt;
            public Conversation nextConversation;
        }
    }
}
