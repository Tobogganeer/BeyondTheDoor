using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

        public static List<Line> Filter(List<Line> toFilter, Line.Element elements)
        {

        }

        static List<Line> FilterPredicate<T>(List<Line> toFilter, T obj, Func<Line, T, bool> comparison)
        {
            // There has got to be a LINQ method for this
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (comparison(line, obj))
                    result.Add(line);

            return result;
        }

        public static IEnumerable<Line> FilterCharacter(IEnumerable<Line> toFilter, CharacterID character)
        {
            return from line in toFilter where line.character == character select line;
        }

        public static List<Line> FilterText(List<Line> toFilter, string text)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (line.text.Contains(text, System.StringComparison.OrdinalIgnoreCase))
                    result.Add(line);

            return result;
        }

        public static List<Line> FilterContext(List<Line> toFilter, string context)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (line.context.Contains(context, System.StringComparison.OrdinalIgnoreCase))
                    result.Add(line);

            return result;
        }

        public static List<Line> FilterDay(List<Line> toFilter, int day)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (line.day == day)
                    result.Add(line);

            return result;
        }

        public static List<Line> FilterLineStatus(List<Line> toFilter, LineStatus lineStatus)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (line.lineStatus == lineStatus)
                    result.Add(line);

            return result;
        }

        public static List<Line> FilterExtraData(List<Line> toFilter, string extraData)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in toFilter)
                if (line.text == text)
                    result.Add(line);

            return result;
        }
    }
}
