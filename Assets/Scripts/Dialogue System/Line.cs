using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Let the importer edit the values on lines directly
[assembly: InternalsVisibleTo("Importer")]

namespace ToBOE.Dialogue
{
    /// <summary>
    /// Represents a line of character dialogue.
    /// </summary>
    [System.Serializable]
    public class Line
    {
        #region Property Accessors
        /// <summary>
        /// The character that speaks this line
        /// </summary>
        public Character Character => character;
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
        public LineStatus VoiceStatus => voiceStatus;
        /// <summary>
        /// Any extra data about the line.
        /// </summary>
        public string ExtraData => extraData;
        #endregion

        [SerializeField] internal Character character;
        [SerializeField] internal string text;
        [SerializeField] internal string context;
        [SerializeField] internal int day;
        [SerializeField] internal LineID id;
        [SerializeField] internal LineStatus lineStatus;
        [SerializeField] internal LineStatus voiceStatus;
        [SerializeField] internal string extraData;

        internal Line() { }

        public Line(Character character, string text, string context, LineID id, LineStatus lineStatus, LineStatus voiceStatus, string extraData)
        {
            this.character = character;
            this.text = text;
            this.context = context;
            this.id = id;
            this.lineStatus = lineStatus;
            this.voiceStatus = voiceStatus;
            this.extraData = extraData;
        }
    }
}
