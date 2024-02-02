using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.TextCore.Text;
using Unity.IO.LowLevel.Unsafe;

namespace ToBOE.Dialogue
{
    public static class LineSearch
    {
        /*
        None = 0,
        Character = 1 << 0,
        Text = 1 << 1,
        Context = 1 << 2,
        Day = 1 << 3,
        LineID = 1 << 4,
        LineStatus = 1 << 5,
        VoiceStatus = 1 << 6,
        ExtraData = 1 << 7
        */

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> that match the <paramref name="elements"/> specified.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="elements">The elements that will be matched/filtered.</param>
        /// <param name="character">If applicable, the character to match with.</param>
        /// <param name="text">If applicable, the text to match with.</param>
        /// <param name="context">If applicable, the context to match with.</param>
        /// <param name="day">If applicable, the day to match with.</param>
        /// <param name="lineStatus">If applicable, the line status to match with.</param>
        /// <param name="voiceStatus">If applicable, the voice status to match with.</param>
        /// <param name="extraData">If applicable, the extra data to match with.</param>
        /// <returns>The lines that match all given <paramref name="elements"/>.</returns>
        public static IEnumerable<Line> Filter(IEnumerable<Line> toFilter, Line.Element elements,
            CharacterID character, string text, string context, int day,
            LineStatus lineStatus, VoiceStatus voiceStatus, string extraData)
        {
            if (elements == Line.Element.None)
                return Enumerable.Empty<Line>();

            // Filter all elements in an optimized order
            if (elements.HasFlag(Line.Element.CharacterID))
                toFilter = FilterCharacter(toFilter, character);
            if (elements.HasFlag(Line.Element.Day))
                toFilter = FilterDay(toFilter, day);
            if (elements.HasFlag(Line.Element.LineStatus))
                toFilter = FilterLineStatus(toFilter, lineStatus);
            if (elements.HasFlag(Line.Element.VoiceStatus))
                toFilter = FilterVoiceStatus(toFilter, voiceStatus);
            if (elements.HasFlag(Line.Element.Text))
                toFilter = FilterText(toFilter, text);
            if (elements.HasFlag(Line.Element.Context))
                toFilter = FilterContext(toFilter, context);
            if (elements.HasFlag(Line.Element.ExtraData))
                toFilter = FilterExtraData(toFilter, extraData);

            return toFilter;
        }

        // There is a LINQ method, hope it doesn't kill performance tho
        // Only a few hundred elements max anyways
        /*
        static List<Line> FilterPredicate<T>(List<Line> toFilter, T obj, Func<Line, T, bool> comparison)
        {
            // There has got to be a LINQ method for this
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (comparison(line, obj))
                    result.Add(line);

            return result;
        }
        */

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> that belong to <paramref name="character"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="character">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterCharacter(IEnumerable<Line> toFilter, CharacterID character)
        {
            return from line in toFilter where line.character == character select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> whose text contains <paramref name="text"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="text">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterText(IEnumerable<Line> toFilter, string text)
        {
            return from line in toFilter where line.text.Contains(text, StringComparison.OrdinalIgnoreCase) select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> whose context contains <paramref name="context"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="context">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterContext(IEnumerable<Line> toFilter, string context)
        {
            return from line in toFilter where line.context.Contains(context, StringComparison.OrdinalIgnoreCase) select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> that are spoken on <paramref name="day"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="day">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterDay(IEnumerable<Line> toFilter, int day)
        {
            return from line in toFilter where line.day == day select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> that match the <paramref name="lineStatus"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="lineStatus">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterLineStatus(IEnumerable<Line> toFilter, LineStatus lineStatus)
        {
            return from line in toFilter where line.lineStatus == lineStatus select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> that match the <paramref name="voiceStatus"/>.
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="voiceStatus">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterVoiceStatus(IEnumerable<Line> toFilter, VoiceStatus voiceStatus)
        {
            return from line in toFilter where line.voiceStatus == voiceStatus select line;
        }

        /// <summary>
        /// Returns the lines from <paramref name="toFilter"/> whose extra data contains <paramref name="extraData"/>
        /// </summary>
        /// <param name="toFilter">The lines to search through.</param>
        /// <param name="extraData">The value to match.</param>
        /// <returns>The lines that match.</returns>
        public static IEnumerable<Line> FilterExtraData(IEnumerable<Line> toFilter, string extraData)
        {
            return from line in toFilter where line.extraData.Contains(extraData, StringComparison.OrdinalIgnoreCase) select line;
        }
    }
}
