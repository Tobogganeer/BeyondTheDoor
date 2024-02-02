using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToBOE.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject, IDialogueElement
    {
        [SerializeField] internal List<LineID> lines;
        [SerializeField] internal List<ConversationChoice> choices;

        private List<Line> _linesBacking;
        private ChoiceCollection _choicesBacking;

        /// <summary>
        /// The lines of this conversation, in order.
        /// </summary>
        public List<Line> Lines
        {
            get
            {
                if (_linesBacking == null || _linesBacking.Count != lines.Count)
                {
                    _linesBacking = new List<Line>(lines.Count);
                    foreach (LineID id in lines)
                        _linesBacking.Add(Line.Get(id));

                    // Make the lines lead into each other
                    if (_linesBacking.Count > 0)
                    {
                        for (int i = 0; i < _linesBacking.Count - 1; i++)
                            _linesBacking[i].Then(_linesBacking[i + 1]);
                        // Make the last one lead to the choices
                        _linesBacking[_linesBacking.Count - 1].Then(Choices);
                    }
                }

                return _linesBacking;
            }
        }
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

        public void Open()
        {
            OnStarted?.Invoke(this);
            if (Lines.Count > 0)
                Lines[0].Open();
        }


        [Serializable]
        internal class ConversationChoice
        {
            public LineID prompt;
            public Conversation nextConversation;
        }
    }
}
