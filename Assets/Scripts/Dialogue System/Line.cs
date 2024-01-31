using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ToBOE.Dialogue
{
    /// <summary>
    /// Represents a line of character dialogue.
    /// </summary>
    [System.Serializable]
    public class Line
    {
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


        [SerializeField] private Character character;
        [SerializeField] private string text;
        [SerializeField] private string context;
        [SerializeField] private LineID id;
        [SerializeField] private LineStatus lineStatus;
        [SerializeField] private LineStatus voiceStatus;
        [SerializeField] private string extraData;

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
