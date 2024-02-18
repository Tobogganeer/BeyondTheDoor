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

        [Header("Config")]
        [SerializeField] private bool startAsDefault = true;

        [Header("GUI Components")]
        [SerializeField] private GameObject dialogueUIContainer;
        [Space]
        [SerializeField] private GameObject characterUIContainer;
        [SerializeField] private TMP_Text characterNameField;
        [SerializeField] private TMP_Text lineTextField;
        [Space]
        [SerializeField] private GameObject choiceUIContainer;
        [SerializeField] private ChoiceGUI[] choiceButtons;

        private Line _currentLine;
        public static Line CurrentLine => Current._currentLine;

        private ChoiceCollection _currentChoices;
        public ChoiceCollection CurrentChoices => _currentChoices;


        private void Start()
        {
            if (startAsDefault)
                SetAsCurrent();

            // Turn us off
            SetWindowActive(false);
        }

        internal void OpenLine(Line line)
        {
            // Turn the line text on, keep choices off
            SetWindowActive(true);
            SetLineTextActive(true);
            SetChoicesActive(false);

            _currentLine = line;

            // TODO: This is for testing
            // TODO: Use Character class to get proper name
            characterNameField.text = line.character.ToString();
            lineTextField.text = line.Text;

            // TODO: Call only when the line is actually done displaying
            line.OnLineClosing();
        }

        internal void OpenChoices(ChoiceCollection choices)
        {
            // Turn the choices on, don't touch the line text
            SetWindowActive(true);
            SetChoicesActive(true);

            // TODO: Call only when the choice is actually made + choose the right one
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


        private void SetWindowActive(bool active) => dialogueUIContainer.SetActive(active);
        private void SetLineTextActive(bool active) => characterUIContainer.SetActive(active);
        private void SetChoicesActive(bool active) => choiceUIContainer.SetActive(active);

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
