using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.Importer
{
    /// <summary>
    /// Represents the minimally cleaned TSV line data (no empty lines, entries are trimmed, blatantly invalid lines removed)
    /// </summary>
    public class TSVData
    {
        /// <summary>
        /// The CharacterID or the marker of special elements (Conversations, Conditionals, etc)
        /// </summary>
        public List<string> CharacterIDColumn { get; private set; }
        /// <summary>
        /// The spoken text, or the next Conversation (choice and goto) or the conditional (if, elif)
        /// </summary>
        public List<string> TextColumn { get; private set; }
        public List<string> ContextColumn { get; private set; }
        public List<string> DayColumn { get; private set; }
        public List<string> LineIDColumn { get; private set; }
        public List<string> LineStatusColumn { get; private set; }
        public List<string> VoiceStatusColumn { get; private set; }
        public List<string> ExtraDataColumn { get; private set; }

        public TSVData(int estimatedElements)
        {
            CharacterIDColumn = new List<string>(estimatedElements);
            TextColumn = new List<string>(estimatedElements);
            ContextColumn = new List<string>(estimatedElements);
            DayColumn = new List<string>(estimatedElements);
            LineIDColumn = new List<string>(estimatedElements);
            LineStatusColumn = new List<string>(estimatedElements);
            VoiceStatusColumn = new List<string>(estimatedElements);
            ExtraDataColumn = new List<string>(estimatedElements);
        }

        public void AddLine(string csvLine, Dictionary<int, Line.Element> mappings)
        {
            RawLineData line = GenerateLineData(csvLine, mappings);
            // If the line is the bare minimum to be considered valid, store it.
            if (LineIsValid(line))
                AddLineToStorage(line);
        }

        RawLineData GenerateLineData(string rawLine, Dictionary<int, Line.Element> mappings)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                return null;

            RawLineData line = new RawLineData();
            string[] elements = rawLine.Split('\t', StringSplitOptions.None);
            for (int i = 0; i < elements.Length; i++)
                FillValue(line, mappings[i], elements[i]);

            // Trim all entries; don't validate every line
            line.Validate(false);
            return line;
        }

        void FillValue(RawLineData line, Line.Element type, string data)
        {
            switch (type)
            {
                case Line.Element.CharacterID:
                    line.character = data;
                    break;
                case Line.Element.Text:
                    line.text = data;
                    break;
                case Line.Element.Context:
                    line.context = data;
                    break;
                case Line.Element.Day:
                    line.day = data;
                    break;
                case Line.Element.LineID:
                    line.id = data;
                    break;
                case Line.Element.LineStatus:
                    line.lineStatus = data;
                    break;
                case Line.Element.VoiceStatus:
                    line.voiceStatus = data;
                    break;
                case Line.Element.ExtraData:
                    line.extraData = data;
                    break;
                default:
                    Debug.LogWarning("Invalid TSV data type " + line.id + " (" + type + ")");
                    break;
            }
        }

        bool LineIsValid(RawLineData line)
        {
            // Every valid line has at least a CharacterID or LineID
            return !string.IsNullOrEmpty(line.character) || !string.IsNullOrEmpty(line.id);
        }

        void AddLineToStorage(RawLineData line)
        {
            CharacterIDColumn.Add(line.character);
            TextColumn.Add(line.text);
            ContextColumn.Add(line.context);
            DayColumn.Add(line.day);
            LineIDColumn.Add(line.id);
            LineStatusColumn.Add(line.lineStatus);
            VoiceStatusColumn.Add(line.voiceStatus);
            ExtraDataColumn.Add(line.extraData);
        }
    }
}
