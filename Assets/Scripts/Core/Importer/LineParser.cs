using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor.Importer
{
    public static class LineParser
    {
        public static RawLineCollection ParseRawLines(string tsvFile)
        {
            // Split the tsv into rows
            tsvFile = tsvFile.Trim();
            string[] rawLines = tsvFile.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // See how the data is stored
            List<RawLineData> lines = new List<RawLineData>(rawLines.Length - 1);
            var mappings = GenerateMappingDictionary(rawLines[0]);

            // Skip the first line (the header)
            for (int i = 1; i < rawLines.Length; i++)
            {
                RawLineData data = GenerateLineData(rawLines[i], mappings);
                if (data != null)
                    lines.Add(data);
            }

            return new RawLineCollection(lines);
        }

        static RawLineData GenerateLineData(string rawLine, Dictionary<int, Line.Element> mappings)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                return null;

            RawLineData line = new RawLineData();
            string[] elements = rawLine.Split('\t', StringSplitOptions.None);
            for (int i = 0; i < elements.Length; i++)
                FillValue(line, mappings[i], elements[i]);

            // Comments in excel file
            if (line.character == "//")
                return null;

            line.Validate();
            return line;
        }

        static void FillValue(RawLineData line, Line.Element type, string data)
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
                    Debug.LogWarning("Invalid data type for RawLineData " + line.id + " (" + type + ")");
                    break;
            }
        }

        /// <summary>
        /// Generates a map of the index of arguments to their type.
        /// </summary>
        /// <param name="mappingLine"></param>
        /// <returns></returns>
        static Dictionary<int, Line.Element> GenerateMappingDictionary(string mappingLine)
        {
            Dictionary<int, Line.Element> mappings = new Dictionary<int, Line.Element>();
            string[] rawMappings = mappingLine.Split('\t');

            for (int i = 0; i < rawMappings.Length; i++)
            {
                if (Enum.TryParse(rawMappings[i], true, out Line.Element type))
                {
                    mappings.Add(i, type);
                }
            }

            int numDataTypes = Enum.GetNames(typeof(Line.Element)).Length - 1; // Subtract 'None'
            if (mappings.Count < numDataTypes)
                throw new FormatException($"TSV file contained less than {numDataTypes} headers!");

            return mappings;
        }

        [Serializable]
        public class RawLineData
        {
            public string character;
            public string text;
            public string context;
            public string day;
            public string id;
            public string lineStatus;
            public string voiceStatus;
            public string extraData;

            public bool IsValid { get; private set; }
            public Line.Element InvalidElements { get; private set; }


            public void Validate()
            {
                // Clean up the data
                character = character?.Trim();
                text = text?.Trim();
                context = context?.Trim();
                day = day?.Trim();
                id = id?.Trim();
                lineStatus = lineStatus?.Trim();
                voiceStatus = voiceStatus?.Trim();
                extraData = extraData?.Trim();

                const string NoneString = "None";

                // Handle empty statuses to be None
                if (string.IsNullOrEmpty(lineStatus))
                    lineStatus = NoneString;
                if (string.IsNullOrEmpty(voiceStatus))
                    voiceStatus = NoneString;

                InvalidElements = GetInvalidElements();
                IsValid = InvalidElements == Line.Element.None;
            }

            private Line.Element GetInvalidElements()
            {
                Line.Element invalidElements = Line.Element.None;
                if (!IsCharacterValid()) invalidElements |= Line.Element.CharacterID;
                if (!IsTextValid()) invalidElements |= Line.Element.Text;
                if (!IsContextValid()) invalidElements |= Line.Element.Context;
                if (!IsDayValid()) invalidElements |= Line.Element.Day;
                if (!IsIDValid()) invalidElements |= Line.Element.LineID;
                if (!IsLineStatusValid()) invalidElements |= Line.Element.LineStatus;
                if (!IsVoiceStatusValid()) invalidElements |= Line.Element.VoiceStatus;
                if (!IsExtraDataValid()) invalidElements |= Line.Element.ExtraData;

                return invalidElements;
            }

            public string GetInvalidElementsString()
            {
                if (IsIDValid())
                    return $"Line '{id}': {InvalidElements}";
                else
                    return $"Line (invalid ID, char={character},text={text}): {InvalidElements}";
                //return $"Raw Line with ID {id} is invalid. Invalid elements: {InvalidElements}.";
            }


            public bool IsCharacterValid() => Enum.TryParse<CharacterID>(character, out _);
            public bool IsTextValid() => !string.IsNullOrEmpty(text);
            public bool IsContextValid() => true;
            public bool IsDayValid() => uint.TryParse(day, out _);
            public bool IsIDValid() => Enum.TryParse<LineID>(id, out _);
            public bool IsLineStatusValid() => Enum.TryParse<LineStatus>(lineStatus, out _);
            public bool IsVoiceStatusValid() => Enum.TryParse<VoiceStatus>(voiceStatus, out _);
            public bool IsExtraDataValid() => true;


            public string GetData(Line.Element type)
            {
                return type switch
                {
                    Line.Element.CharacterID => character,
                    Line.Element.Text => text,
                    Line.Element.Context => context,
                    Line.Element.Day => day,
                    Line.Element.LineID => id,
                    Line.Element.LineStatus => lineStatus,
                    Line.Element.VoiceStatus => voiceStatus,
                    Line.Element.ExtraData => extraData,
                    _ => throw new ArgumentException("Invalid LineDataType", "type")
                };
            }
        }

        [Serializable]
        public class RawLineCollection
        {
            public List<RawLineData> RawLines { get; private set; }
            public List<RawLineData> InvalidLines { get; private set; }
            public Line.Element InvalidElements { get; private set; }
            public bool IsValid { get; private set; }

            public RawLineCollection(List<RawLineData> rawLines)
            {
                RawLines = rawLines;
                InvalidLines = new List<RawLineData>();

                InvalidElements = Line.Element.None;
                foreach (RawLineData line in rawLines)
                {
                    InvalidElements |= line.InvalidElements;
                    if (!line.IsValid)
                        InvalidLines.Add(line);
                }

                IsValid = InvalidElements == Line.Element.None;
            }

            /// <summary>
            /// Gets a list of the <paramref name="type"/> data from all lines, e.g. the Character of all lines.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public List<string> GetAllData(Line.Element type, bool removeDuplicates = true)
            {
                if (removeDuplicates)
                    return GetAllDataNoDuplicates(type);
                return GetAllDataIncludingDuplicates(type);
            }

            List<string> GetAllDataNoDuplicates(Line.Element type)
            {
                List<string> data = new List<string>(RawLines.Count);
                for (int i = 0; i < RawLines.Count; i++)
                {
                    string entry = RawLines[i].GetData(type);
                    if (!data.Contains(entry))
                        data.Add(entry);
                }

                return data;
            }

            List<string> GetAllDataIncludingDuplicates(Line.Element type)
            {
                List<string> data = new List<string>(RawLines.Count);
                for (int i = 0; i < RawLines.Count; i++)
                    data.Add(RawLines[i].GetData(type));

                return data;
            }
        }
    }
}
