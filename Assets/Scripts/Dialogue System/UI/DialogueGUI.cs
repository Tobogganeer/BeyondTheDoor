using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ToBOE.Dialogue.UI
{
    public class DialogueGUI : MonoBehaviour
    {
        public static DialogueGUI Current { get; private set; }
        public bool IsCurrent => Current == this;

        [SerializeField] private TMP_Text characterNameField;
        [SerializeField] private TMP_Text textField;
        [SerializeField] private GameObject dialogueUIContainer;
        [Space]
        [SerializeField] private GameObject choiceUIContainer;
        [SerializeField] private ChoiceGUI[] choiceButtons;


        internal void OpenLine(Line line)
        {
            // TODO: Display stuff

            // TODO: Call only when the line is actually done displaying
            line.OnLineClosing();
        }

        internal void OpenChoices(ChoiceCollection choices)
        {
            // TODO: Display stuff and make the choice

            // TODO: Call only when the choice is actually made
            choices.Choices[0].OnChoiceChosen();
        }

        #region Static Accessors
        internal static void Open(Line line)
        {
            if (Current == null)
                throw new System.InvalidOperationException("Tried to open line with no active DialogueGUI.");

            Current.OpenLine(line);
        }

        internal static void Open(ChoiceCollection choices)
        {
            if (Current == null)
                throw new System.InvalidOperationException("Tried to open choices with no active DialogueGUI.");

            Current.OpenChoices(choices);
        }
        #endregion

        public void SetAsCurrent()
        {
            Current = this;
        }


        [System.Serializable]
        public class ChoiceGUI
        {
            public Button button;
            public TMP_Text label;
        }
    }
}
