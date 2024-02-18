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
        [Range(5f, 20f)]
        [SerializeField] private float revealedCharactersPerSecond = 10f;

        [Header("GUI Components")]
        [SerializeField] private GameObject dialogueUIContainer;
        [Space]
        [SerializeField] private GameObject characterUIContainer;
        [SerializeField] private TMP_Text characterNameField;
        [SerializeField] private TMP_Text lineTextField;
        [Space]
        [SerializeField] private GameObject choiceUIContainer;
        [SerializeField] private ChoiceGUI[] choiceButtons;

        private Line currentLine;
        private ChoiceCollection currentChoices;
        private int revealedLength;

        public static Line CurrentLine => Current.currentLine;
        public ChoiceCollection CurrentChoices => currentChoices;

        float revealTimer;


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

            SetNewLine(line);
        }

        internal void OpenChoices(ChoiceCollection choices)
        {
            // Turn the choices on, don't touch the line text
            SetWindowActive(true);
            SetChoicesActive(true);

            currentChoices = choices;

            // TODO: Call only when the choice is actually made + choose the right one
            choices.Choices[0].OnChoiceChosen();
        }

        private void SetNewLine(Line line)
        {
            // Get the incremental printing ready
            currentLine = line;
            revealedLength = 0;
            ResetRevealTimer();

            // TODO: This is for testing
            // TODO: Use Character class to get proper name
            characterNameField.text = line.character.ToString();
            lineTextField.text = line.Text;

            // TODO: Call only when the line is actually done displaying
            line.OnLineClosing();
        }

        private void ResetRevealTimer() => revealTimer = 1f / revealedCharactersPerSecond;

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

        /// <summary>
        /// Closes the window and stops the dialogue.
        /// </summary>
        public static void Close()
        {
            if (Current != null)
                Current._Close();
        }

        /// <summary>
        /// Skips to the end of the current line or advances to the next one.
        /// </summary>
        public static void Next()
        {
            if (Current != null)
                Current._Next();
        }
        #endregion


        private void SetWindowActive(bool active) => dialogueUIContainer.SetActive(active);
        private void SetLineTextActive(bool active) => characterUIContainer.SetActive(active);
        private void SetChoicesActive(bool active) => choiceUIContainer.SetActive(active);

        public void SetAsCurrent()
        {
            Current = this;
        }

        private void _Close()
        {
            SetWindowActive(false);
            currentLine = null;
            currentChoices = null;
        }

        private void _Next()
        {
            // No line to skip ._.
            if (currentLine == null)
                return;


        }


        [System.Serializable]
        public class ChoiceGUI
        {
            public Button button;
            public TMP_Text label;
        }
    }
}
