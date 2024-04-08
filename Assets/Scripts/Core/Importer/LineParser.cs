using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor.Importer
{
    public static class LineParser
    {
        // List of symbols that should be ignored
        static readonly string Comment = "//";
        static readonly HashSet<string> ReservedIDs = new HashSet<string>(new string[] {
            "conversation",
            "end",
            "if",
            "elif",
            "else if",
            "else",
            "endif",
            "end if",
            "choice",
            "goto",
        });

        /// <summary>
        /// Splits the TSV file into all lines worth considering and cleans them up.
        /// </summary>
        /// <param name="tsvFile"></param>
        /// <returns></returns>
        public static TSVData ParseLines(string tsvFile)
        {
            // Split the tsv into rows
            //tsvFile = tsvFile.Trim();
            string[] rawLines = tsvFile.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // See how the data is stored
            var mappings = GenerateMappingDictionary(rawLines[0]);
            TSVData cleanData = new TSVData(rawLines.Length);

            // Skip the first line (the header)
            for (int i = 1; i < rawLines.Length; i++)
            {
                cleanData.AddLine(rawLines[0], mappings);
            }

            return cleanData;
        }

        static RawLineData GenerateLineData(string rawLine, Dictionary<int, Line.Element> mappings)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                return null;

            RawLineData line = new RawLineData();
            string[] elements = rawLine.Split('\t', StringSplitOptions.None);
            for (int i = 0; i < elements.Length; i++)
                FillValue(line, mappings[i], elements[i]);

            // Ignore this line if it is a special marker
            string characterValue = line.character?.Trim().ToLower();
            if (characterValue.StartsWith(Comment) || ReservedIDs.Contains(characterValue))
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
    }
}
