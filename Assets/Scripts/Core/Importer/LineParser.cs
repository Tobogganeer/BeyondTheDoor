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
                cleanData.AddLine(rawLines[i], mappings);
            }

            return cleanData;
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

        public static RawLineCollection ParseRawLines(TSVData data)
        {
            List<RawLineData> rawLines = new List<RawLineData>(data.Count);
            HashSet<string> seenLineIDs = new HashSet<string>(data.Count);

            foreach (RawLineData potentialLine in data.GetRawLines())
            {
                // Ignore this line if it is a special marker
                if (!CharacterValueValid(potentialLine))
                    continue;

                // Only process lines once
                if (seenLineIDs.Contains(potentialLine.id))
                    continue;

                // Discard lines that will cause compile errors
                if (!HasRequiredLineValues(potentialLine))
                {
                    potentialLine.Validate();
                    Debug.LogWarning("Discarding line for missing required elements.\n"
                        + potentialLine.GetInvalidElementsString());
                    continue;
                }

                // Store each good line
                potentialLine.Validate();
                rawLines.Add(potentialLine);
                seenLineIDs.Add(potentialLine.id);
            }

            return new RawLineCollection(rawLines);
        }

        static bool CharacterValueValid(RawLineData potentialLine)
        {
            // Ignore this line if it is a special marker or is empty
            string characterValue = potentialLine.character.ToLower();
            return !string.IsNullOrEmpty(characterValue) &&
                !characterValue.StartsWith(Comment) &&
                !ReservedIDs.Contains(characterValue);
        }

        static bool HasRequiredLineValues(RawLineData potentialLine)
        {
            return !string.IsNullOrEmpty(potentialLine.character) &&
                !string.IsNullOrEmpty(potentialLine.day) &&
                !string.IsNullOrEmpty(potentialLine.id);
        }
    }
}
