using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BeyondTheDoor.UI
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
        [SerializeField] private GameObject endOfLineMarker;
        [Space]
        [SerializeField] private GameObject choiceUIContainer;
        [SerializeField] private ChoiceGUI[] choiceButtons;

        private Line currentLine;
        private string formattedLineText;
        private ChoiceCollection currentChoices;
        private int revealedLength;

        /// <summary>
        /// Set by Settings.cs
        /// </summary>
        public static float RevealedCharactersPerSecond { get; set; }

        public static Line CurrentLine => Current?.currentLine;
        public static ChoiceCollection CurrentChoices => Current?.currentChoices;
        public static bool HasLine => CurrentLine != null;
        public static bool HasChoices => CurrentChoices != null;
        public static bool AtEndOfLine => HasLine && Current.revealedLength >= Current.formattedLineText.Length;
        public static bool IsOpen => HasLine;

        // Intended to be used for audio VVV
        /// <summary>
        /// Invoked when a line has started being displayed.
        /// </summary>
        public static event Action<LineID> OnLineStart;
        /// <summary>
        /// Invoked when a line is skipped or the next line is started.
        /// </summary>
        public static event Action<LineID> OnLineStop;


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

            // If we are waiting for input, let the player know
            endOfLineMarker.SetActive(AtEndOfLine && !HasChoices);
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

            OnLineStart?.Invoke(line.ID);
        }


        internal void OpenChoices(ChoiceCollection choices)
        {
            // Turn the choices on, don't touch the line text
            SetWindowActive(true);
            SetChoicesActive(true);

            SetChoices(choices);

            // TODO: Call only when the choice is actually made + choose the right one
            //choices.Choices[0].OnChoiceChosen();
        }

        private void SetChoices(ChoiceCollection choices)
        {
            // Close if trying to display no choices
            if (choices.Choices.Count == 0)
                Close();

            currentChoices = choices;
            if (choices.Choices.Count > choiceButtons.Length)
                Debug.LogWarning("Too many choices to present, will be truncated. First line: " + choices.Choices[0].Prompt.ToString());

            int numChoices = Mathf.Min(choices.Choices.Count, choiceButtons.Length);
            // Disable the unneeded buttons
            for (int i = 0; i < choiceButtons.Length; i++)
                choiceButtons[i].button.gameObject.SetActive(i < numChoices);

            // Wire the buttons to the correct choices
            for (int i = 0; i < numChoices; i++)
                SetChoiceButton(choices.Choices[i], choiceButtons[i]);
        }

        private void SetChoiceButton(Choice choice, ChoiceGUI gui)
        {
            if (choice == null)
                throw new System.ArgumentNullException(nameof(choice));

            gui.label.text = choice.Prompt.Text; // Set the right text
            gui.button.onClick.RemoveAllListeners(); // Clear the previous choices
            gui.button.onClick.AddListener(() => OnChoiceChosen(choice)); // Wire it up
        }

        private void OnChoiceChosen(Choice choice)
        {
            currentChoices = null; // Remove our choices
            choice.OnChoiceChosen(); // Activate it
        }


        private void ResetRevealTimer() => revealTimer = 1f / RevealedCharactersPerSecond;

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

        /// <summary>
        /// Sets this dialogue GUI as the current GUI.
        /// </summary>
        public void SetAsCurrent()
        {
            Current = this;
        }

        private void _Close()
        {
            SetWindowActive(false);

            if (HasLine && !AtEndOfLine)
                OnLineStop?.Invoke(CurrentLine.ID);

            currentLine = null;
            currentChoices = null;
        }

        private void _Next()
        {
            // No line to skip ._.
            if (!HasLine)
                return;

            // Don't update our current line if we have choices
            if (HasChoices)
                return;

            OnLineStop?.Invoke(CurrentLine.ID);

            // Let the line know it's done
            if (AtEndOfLine)
                currentLine.OnLineClosing();
            else
            {
                // Skip to the end of the line
                revealedLength = string.IsNullOrEmpty(formattedLineText) ? 0 : formattedLineText.Length;
                UpdateLineTextGUI();
            }
        }


        [System.Serializable]
        private class ChoiceGUI
        {
            public Button button;
            public TMP_Text label;
        }
    }
}
