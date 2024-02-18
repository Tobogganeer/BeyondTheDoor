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
        [SerializeField] private float revealedCharactersPerSecond = 15f;

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
        private string formattedLineText;
        private ChoiceCollection currentChoices;
        private int revealedLength;

        public static Line CurrentLine => Current?.currentLine;
        public static ChoiceCollection CurrentChoices => Current?.currentChoices;
        public static bool HasLine => CurrentLine != null;
        public static bool HasChoices => CurrentChoices != null;
        public static bool AtEndOfLine => HasLine && Current.revealedLength >= Current.formattedLineText.Length;


        float revealTimer;


        private void Start()
        {
            if (startAsDefault)
                SetAsCurrent();

            // Turn us off
            SetWindowActive(false);
        }

        private void Update()
        {
            // Don't update if we aren't the current GUI
            if (!IsCurrent)
                return;

            if (HasLine && !AtEndOfLine)
            {
                UpdateLine();
            }
        }

        private void UpdateLine()
        {
            // Decrease the timer
            revealTimer -= Time.deltaTime;
            if (revealTimer <= 0)
            {
                // Reveal another character
                ResetRevealTimer();
                revealedLength++;
                UpdateLineTextGUI();
            }
        }

        private void UpdateLineTextGUI()
        {
            lineTextField.text = formattedLineText?.Substring(0, revealedLength);
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
            // Set our current
            currentLine = line;
            // TODO: Format text accounting for variables etc
            formattedLineText = line.text;
            // Get the incremental printing ready
            revealedLength = 0;
            ResetRevealTimer();

            // TODO: Use Character class to get proper name
            characterNameField.text = line.character.ToString();
            lineTextField.text = string.Empty; // Blank

            // TODO: Call only when the line is actually done displaying
            //line.OnLineClosing();
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
