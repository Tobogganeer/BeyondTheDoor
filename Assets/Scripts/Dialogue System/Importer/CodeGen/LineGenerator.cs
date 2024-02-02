using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.IO.LowLevel.Unsafe;

namespace ToBOE.Dialogue.Importer.CodeGen
{
    public static class LineGenerator
    {
        public static Line Generate(LineParser.RawLineData rawLine)
        {
            // Make sure it is all good
            if (rawLine == null)
                throw new ArgumentNullException(nameof(rawLine));
            if (!rawLine.IsValid)
                throw new ArgumentException("Passed line was not valid.", nameof(rawLine));

            Line l = new Line();

            // Booooring
            l.character = Enum.Parse<Character>(rawLine.character);
            l.text = rawLine.text;
            l.context = rawLine.context;
            l.day = int.Parse(rawLine.day);
            l.id = Enum.Parse<LineID>(rawLine.id);
            l.lineStatus = Enum.Parse<LineStatus>(rawLine.lineStatus);
            l.voiceStatus = Enum.Parse<VoiceStatus>(rawLine.voiceStatus);

            return l;
        }
    }
}
