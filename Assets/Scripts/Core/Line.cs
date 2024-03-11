using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let the importer and UI edit the values on lines directly
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Importer")]
// And the save system as well :P
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SaveSystem")]

namespace BeyondTheDoor
{
    /// <summary>
    /// Represents a line of character dialogue.
    /// </summary>
    [Serializable]
    public partial class Line : IDialogueElement
    {
        #region Property Accessors
        /// <summary>
        /// The character that speaks this line
        /// </summary>
        public CharacterID CharacterID => character;
        /// <summary>
        /// The text that is spoken and shown on screen.
        /// </summary>
        public string Text => text;
        /// <summary>
        /// Internal context around the line.
        /// </summary>
        public string Context => context;
        /// <summary>
        /// The day on which this line is spoken.
        /// </summary>
        public int Day => day;
        /// <summary>
        /// The ID of the line.
        /// </summary>
        public LineID ID => id;
        /// <summary>
        /// The status of the text.
        /// </summary>
        public LineStatus LineStatus => lineStatus;
        /// <summary>
        /// The status of the voice recording.
        /// </summary>
        public VoiceStatus VoiceStatus => voiceStatus;
        /// <summary>
        /// Any extra data about the line.
        /// </summary>
        public string ExtraData => extraData;

        /// <summary>
        /// How many times this line has been opened/spoken.
        /// </summary>
        public int TimesOpened => timesOpened;
        #endregion

        [SerializeField] internal CharacterID character;
        [SerializeField] internal string text;
        [SerializeField] internal string context;
        [SerializeField] internal int day;
        [SerializeField] internal LineID id;
        [SerializeField] internal LineStatus lineStatus;
        [SerializeField] internal VoiceStatus voiceStatus;
        [SerializeField] internal string extraData;

        private IDialogueElement followupElement;
        internal int timesOpened;

        /// <summary>
        /// Called when this line is opened/displayed.
        /// </summary>
        public event Action<Line> OnOpen;
        /// <summary>
        /// Called after this line is closed/been moved on from.
        /// </summary>
        public event Action<Line> OnClose;

        internal Line() { }
        public Line(CharacterID character, string text, string context, int day, LineID id, LineStatus lineStatus, VoiceStatus voiceStatus, string extraData)
        {
            this.character = character;
            this.text = text;
            this.context = context;
            this.day = day;
            this.id = id;
            this.lineStatus = lineStatus;
            this.voiceStatus = voiceStatus;
            this.extraData = extraData;
        }


        public void Open()
        {
            timesOpened++;
            Character.Current = Character.All[character];
            OnOpen?.Invoke(this);
            UI.DialogueGUI.Open(this);
        }

        /// <summary>
        /// Opens the <paramref name="followingLine"/> after this line is complete.
        /// </summary>
        /// <param name="followingLine">The line to open after this one.</param>
        /// <returns></returns>
        public Line Then(Line followingLine)
        {
            followupElement = followingLine;
            return followingLine;
        }

        /// <summary>
        /// Opens the <paramref name="followingElement"/> after this line is complete.
        /// </summary>
        /// <param name="followingElement">The element to open after this one.</param>
        /// <returns></returns>
        public void Then(IDialogueElement followingElement)
        {
            followupElement = followingElement;
        }

        /// <summary>
        /// Lets the player choose between the <paramref name="choices"/> provided.
        /// </summary>
        /// <param name="choices">The possible choices.</param>
        public void ThenChoice(params Choice[] choices)
        {
            ChoiceCollection choiceCollection = new ChoiceCollection(choices);
            followupElement = choiceCollection;
        }



        /// <summary>
        /// Gets the Line with the specified <paramref name="id"/>. Shorthand for Line.All[<paramref name="id"/>];
        /// </summary>
        /// <param name="id">The ID of the line to get.</param>
        public static Line Get(LineID id)
        {
            if (All.TryGetValue(id, out Line val))
                return val;
            return null;
        }

        /// <summary>
        /// Returns true if a line with the given <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Exists(LineID id) => All.ContainsKey(id);



        /// <summary>
        /// Called when this line is finished displaying.
        /// </summary>
        internal void OnLineClosing()
        {
            // Open the next thing if there is one
            if (followupElement != null)
                followupElement.Open();
            // If not just stop the dialogue
            else
                UI.DialogueGUI.Close();

            OnClose?.Invoke(this);
        }


        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("ID -> ").Append(id.ToString()).Append(" - Text -> ");
            sb.Append(character.ToString()).Append(": ").Append(text);
            sb.Append(" (Day ").Append(day).Append(")");
            if (string.IsNullOrEmpty(context))
                sb.Append(" - no context");
            else
                sb.Append("- context -> ").Append(context);
            sb.Append(" - line status -> ").Append(lineStatus.ToString());
            sb.Append(" - voice status -> ").Append(voiceStatus.ToString());
            if (string.IsNullOrEmpty(extraData))
                sb.Append(" - no extra data");
            else
                sb.Append(" - extra data -> ").Append(extraData);

            return sb.ToString();
        }

        [Flags]
        public enum Element
        {
            None = 0,
            CharacterID = 1 << 0,
            Text = 1 << 1,
            Context = 1 << 2,
            Day = 1 << 3,
            LineID = 1 << 4,
            LineStatus = 1 << 5,
            VoiceStatus = 1 << 6,
            ExtraData = 1 << 7
        }
    }
}
