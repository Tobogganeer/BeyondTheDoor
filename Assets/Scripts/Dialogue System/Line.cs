using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let the importer edit the values on lines directly
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Importer")]

namespace ToBOE.Dialogue
{
    /// <summary>
    /// Represents a line of character dialogue.
    /// </summary>
    [System.Serializable]
    public partial class Line : IDialogueElement
    {
        #region Property Accessors
        /// <summary>
        /// The character that speaks this line
        /// </summary>
        public CharacterID Character => character;
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
        private int timesOpened;

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
            DialogueGUI.OpenLine(this);
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
        /// Lets the player choose between the <paramref name="choices"/> provided.
        /// </summary>
        /// <param name="choices">The possible choices.</param>
        public void ThenChoice(params Choice[] choices)
        {
            ChoiceCollection choiceCollection = new ChoiceCollection(choices);
            followupElement = choiceCollection;
        }


        /// <summary>
        /// Called when this line is finished displaying.
        /// </summary>
        internal void OnLineClosing()
        {
            if (followupElement != null)
                followupElement.Open();
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
