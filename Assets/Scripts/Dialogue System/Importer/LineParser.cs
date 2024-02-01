using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToBOE.Dialogue.Importer
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
                lines.Add(GenerateLineData(rawLines[i], mappings));

            return new RawLineCollection(lines);
        }

        static RawLineData GenerateLineData(string rawLine, Dictionary<int, LineDataType> mappings)
        {
            RawLineData line = new RawLineData();
            string[] elements = rawLine.Split('\t', StringSplitOptions.None);
            for (int i = 0; i < elements.Length; i++)
                FillValue(line, mappings[i], elements[i]);

            line.Validate();
            return line;
        }

        static void FillValue(RawLineData line, LineDataType type, string data)
        {
            switch (type)
            {
                case LineDataType.Character:
                    line.character = data;
                    break;
                case LineDataType.Text:
                    line.text = data;
                    break;
                case LineDataType.Context:
                    line.context = data;
                    break;
                case LineDataType.Day:
                    line.day = data;
                    break;
                case LineDataType.LineID:
                    line.id = data;
                    break;
                case LineDataType.LineStatus:
                    line.lineStatus = data;
                    break;
                case LineDataType.VoiceStatus:
                    line.voiceStatus = data;
                    break;
                case LineDataType.ExtraData:
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
        static Dictionary<int, LineDataType> GenerateMappingDictionary(string mappingLine)
        {
            Dictionary<int, LineDataType> mappings = new Dictionary<int, LineDataType>();
            string[] rawMappings = mappingLine.Split('\t');

            for (int i = 0; i < rawMappings.Length; i++)
            {
                if (Enum.TryParse(rawMappings[i], true, out LineDataType type))
                {
                    mappings.Add(i, type);
                }
            }

            int numDataTypes = Enum.GetNames(typeof(LineDataType)).Length - 1; // Subtract 'None'
            if (mappings.Count < numDataTypes)
                throw new FormatException($"TSV file contained less than {numDataTypes} headers!");

            return mappings;
        }

        [Flags]
        public enum LineDataType
        {
            None = 0,
            Character = 1 << 0,
            Text = 1 << 1,
            Context = 1 << 2,
            Day = 1 << 3,
            LineID = 1 << 4,
            LineStatus = 1 << 5,
            VoiceStatus = 1 << 6,
            ExtraData = 1 << 7
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
            public LineDataType InvalidElements { get; private set; }


            public void Validate()
            {
                InvalidElements = GetInvalidElements();
                IsValid = InvalidElements == LineDataType.None;
            }

            private LineDataType GetInvalidElements()
            {
                LineDataType invalidElements = LineDataType.None;
                if (!IsCharacterValid()) invalidElements |= LineDataType.Character;
                if (!IsTextValid()) invalidElements |= LineDataType.Text;
                if (!IsContextValid()) invalidElements |= LineDataType.Context;
                if (!IsDayValid()) invalidElements |= LineDataType.Day;
                if (!IsIDValid()) invalidElements |= LineDataType.LineID;
                if (!IsLineStatusValid()) invalidElements |= LineDataType.LineStatus;
                if (!IsVoiceStatusValid()) invalidElements |= LineDataType.VoiceStatus;
                if (!IsExtraDataValid()) invalidElements |= LineDataType.ExtraData;

                return invalidElements;
            }

            public string GetInvalidElementsString()
            {
                return $"Raw Line with ID {id} is invalid. Invalid elements: {InvalidElements}.";
            }


            public bool IsCharacterValid() => Enum.TryParse<Character>(character, out _);
            public bool IsTextValid() => !string.IsNullOrEmpty(text);
            public bool IsContextValid() => true;
            public bool IsDayValid() => uint.TryParse(day, out _);
            public bool IsIDValid() => Enum.TryParse<LineID>(id, out _);
            public bool IsLineStatusValid() => Enum.TryParse<LineStatus>(lineStatus, out _);
            public bool IsVoiceStatusValid() => Enum.TryParse<VoiceStatus>(voiceStatus, out _);
            public bool IsExtraDataValid() => true;


            public string GetData(LineDataType type)
            {
                return type switch
                {
                    LineDataType.Character => character,
                    LineDataType.Text => text,
                    LineDataType.Context => context,
                    LineDataType.Day => day,
                    LineDataType.LineID => id,
                    LineDataType.LineStatus => lineStatus,
                    LineDataType.VoiceStatus => voiceStatus,
                    LineDataType.ExtraData => extraData,
                    _ => throw new ArgumentException("Invalid LineDataType", "type")
                };
            }
        }

        [Serializable]
        public class RawLineCollection
        {
            public List<RawLineData> RawLines { get; private set; }
            public List<RawLineData> InvalidLines { get; private set; }
            public LineDataType InvalidElements { get; private set; }
            public bool IsValid { get; private set; }

            public RawLineCollection(List<RawLineData> rawLines)
            {
                RawLines = rawLines;
                InvalidLines = new List<RawLineData>();

                InvalidElements = LineDataType.None;
                foreach (RawLineData line in rawLines)
                {
                    InvalidElements |= line.InvalidElements;
                    if (!line.IsValid)
                        InvalidLines.Add(line);
                }

                IsValid = InvalidElements == LineDataType.None;
            }

            /// <summary>
            /// Gets a list of the <paramref name="type"/> data from all lines, e.g. the Character of all lines.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public List<string> GetAllData(LineDataType type, bool removeDuplicates = true)
            {
                if (removeDuplicates)
                    return GetAllDataNoDuplicates(type);
                return GetAllDataIncludingDuplicates(type);
            }

            List<string> GetAllDataNoDuplicates(LineDataType type)
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

            List<string> GetAllDataIncludingDuplicates(LineDataType type)
            {
                List<string> data = new List<string>(RawLines.Count);
                for (int i = 0; i < RawLines.Count; i++)
                    data.Add(RawLines[i].GetData(type));

                return data;
            }
        }
    }
}
