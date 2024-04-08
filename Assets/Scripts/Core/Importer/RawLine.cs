using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.Importer
{
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


        public void Validate(bool calculateInvalidElements = true)
        {
            // Clean up the data
            character = character?.Trim() ?? string.Empty;
            text = text?.Trim() ?? string.Empty;
            context = context?.Trim() ?? string.Empty;
            day = day?.Trim() ?? string.Empty;
            id = id?.Trim() ?? string.Empty;
            lineStatus = lineStatus?.Trim() ?? string.Empty;
            voiceStatus = voiceStatus?.Trim() ?? string.Empty;
            extraData = extraData?.Trim() ?? string.Empty;

            const string NoneString = "None";

            // Handle empty statuses to be None
            if (string.IsNullOrEmpty(lineStatus))
                lineStatus = NoneString;
            if (string.IsNullOrEmpty(voiceStatus))
                voiceStatus = NoneString;

            if (calculateInvalidElements)
            {
                InvalidElements = GetInvalidElements();
                IsValid = InvalidElements == Line.Element.None;
            }
        }

        /// <summary>
        /// Clears artifacts from the TSV export (triple quotes, random single quotes)
        /// </summary>
        /// <remarks>Do not call this twice! It will remove valid quotation marks!</remarks>
        public void CorrectQuotationMarks()
        {
            string tripleQuote = "\"\"\"";
            string quote = "\"";
            // Triple quotes means we want a quote at the start and end
            if (text.StartsWith(tripleQuote) && text.EndsWith(tripleQuote))
                text = text.Replace(tripleQuote, quote);
            // Some lines have random quotes placed on them
            else if (text.StartsWith(quote) && text.EndsWith(quote))
                text = text.Replace(quote, string.Empty);

            // TODO: This function could format text poorly if it has quotes not at the start and end
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
