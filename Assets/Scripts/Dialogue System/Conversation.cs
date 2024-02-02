using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToBOE.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Conversation : ScriptableObject, IDialogueElement
    {
        [SerializeField] private List<LineID> lines;
        [SerializeField] private List<ConversationChoice> choices;

        private List<Line> _linesBacking;
        private ChoiceCollection _choicesBacking;

        public List<Line> Lines
        {
            get
            {
                if (_linesBacking == null || _linesBacking.Count != lines.Count)
                {
                    _linesBacking = new List<Line>(lines.Count);
                    foreach (LineID id in lines)
                        _linesBacking.Add(Line.Get(id));
                }

                return _linesBacking;
            }
        }
        public ChoiceCollection Choices
        {
            get
            {
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
        public event Action<Conversation> OnOpen;

        public void Open()
        {
            OnOpen?.Invoke(this);
            if (Lines.Count > 0)
                Lines[0].Open();
        }


        [Serializable]
        public class ConversationChoice
        {
            public LineID prompt;
            public Conversation nextConversation;
        }
    }
}
