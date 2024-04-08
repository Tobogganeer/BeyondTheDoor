using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace BeyondTheDoor.Importer
{
    /// <summary>
    /// Represents the minimally cleaned TSV line data (no empty lines, entries are trimmed, blatantly invalid lines removed)
    /// </summary>
    public class TSVData
    {
        const string FileName = "TSVData.txt";
        static readonly string FilePath = Path.Combine(Application.dataPath, FileName);
        static readonly Encoding Encoding = Encoding.ASCII;

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
        public int Count => CharacterIDColumn != null ? CharacterIDColumn.Count : 0;

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


        // https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream?view=net-8.0
        public void Save()
        {
            // Delete the file if it exists.
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            //Create the file.
            using (FileStream fs = File.Create(FilePath))
            {
                // Some lines are long; leave enough room
                byte[] stringBuffer = new byte[1024];
                char newLine = '\n';

                // Add how many lines we have
                Write(fs, stringBuffer, Count.ToString() + newLine);

                for (int i = 0; i < Count; i++)
                {
                    // Write all lines in order
                    Write(fs, stringBuffer, CharacterIDColumn[i] + newLine);
                    Write(fs, stringBuffer, TextColumn[i] + newLine);
                    Write(fs, stringBuffer, ContextColumn[i] + newLine);
                    Write(fs, stringBuffer, DayColumn[i] + newLine);
                    Write(fs, stringBuffer, LineIDColumn[i] + newLine);
                    Write(fs, stringBuffer, LineStatusColumn[i] + newLine);
                    Write(fs, stringBuffer, VoiceStatusColumn[i] + newLine);
                    Write(fs, stringBuffer, ExtraDataColumn[i] + newLine);
                }
            }
        }

        void Write(FileStream fs, byte[] buf, string value)
        {
            // Reuse the buffer and write the string
            int written = Encoding.GetBytes(value, 0, value.Length, buf, 0);
            fs.Write(buf, 0, written);
        }


        public static TSVData Load()
        {
            if (!SavedDataExists())
                throw new FileNotFoundException("No saved TSVData file found");

            TSVData data = null;
            bool first = true;
            int index = 0;
            int count = 0;
            const int NumElements = 8;
            List<string> lineBuffer = new List<string>(NumElements);

            foreach (string line in File.ReadLines(FilePath, Encoding))
            {
                if (first)
                {
                    // Read the amount on the first time through
                    first = false;
                    count = int.Parse(line.TrimEnd());
                    data = new TSVData(count);
                }
                else
                {
                    lineBuffer.Add(line);
                    // Add it when it's full
                    if (lineBuffer.Count == NumElements)
                    {
                        AddLineBuffer(data, lineBuffer);
                        index++;
                        // Check if we are done
                        if (index == count)
                            break;
                    }
                }
            }

            return data;
        }

        private static void AddLineBuffer(TSVData to, List<string> lineBuffer)
        {
            RawLineData d = new RawLineData();
            d.character = lineBuffer[0];
            d.text = lineBuffer[1];
            d.context = lineBuffer[2];
            d.day = lineBuffer[3];
            d.id = lineBuffer[4];
            d.lineStatus = lineBuffer[5];
            d.voiceStatus = lineBuffer[6];
            d.extraData = lineBuffer[7];

            to.AddLineToStorage(d);

            lineBuffer.Clear();
        }

        public static bool SavedDataExists()
        {
            return File.Exists(FilePath);
        }
    }
}
