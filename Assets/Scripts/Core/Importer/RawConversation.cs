using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.Importer
{
    public class RawConversationData
    {
        public static readonly string IfMarker = "if";
        public static readonly string ElifMarker = "elif";
        public static readonly string ElseIfMarker = "else if";
        public static readonly string ElseMarker = "else";
        public static readonly string EndIfMarker = "endif";
        public static readonly string End_IfMarker = "end if";
        public static readonly string ChoiceMarker = "choice";
        public static readonly string GotoMarker = "goto";

        public string name;
        public string fileName;
        public int day;
        public List<IConversationElement> elements;
        public List<RawChoice> choices;

        public RawConversationData(string name, int? day)
        {
            this.name = name.Trim();
            this.day = day ?? FindDay(name);

            elements = new List<IConversationElement>();
            choices = new List<RawChoice>();
        }

        static int FindDay(string name)
        {
            // Tries to find a valid day number in the name

            name = name.Trim().ToLower();

            if (string.IsNullOrEmpty(name))
                return -1;

            if (name.Contains("day"))
            {
                int index = name.IndexOf("day");
                // Too lazy to handle errors currently
                try
                {
                    // Check the characters afterwards
                    // i.e. Day3 or DAY_4 or Day 5
                    if (name.Length > index + 3 && char.IsNumber(name[index + 3]))
                        return int.Parse(name[index + 3].ToString());
                    else if (name.Length > index + 4 && char.IsNumber(name[index + 4]))
                        return int.Parse(name[index + 4].ToString());
                    else
                    {
                        for (int i = 0; i < name.Length; i++)
                        {
                            if (char.IsNumber(name[i]))
                            {
                                int num = int.Parse(name[i].ToString());
                                Debug.LogWarning($"Had to guess day number for conversation '{name}'. Guess: " + num);
                                return num;
                            }
                        }
                    }
                }
                catch
                {
                    return -1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the name with the day prepended.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetFormattedName(string name, int? day, out int parsedDay)
        {
            name = name.Trim();
            parsedDay = day ?? FindDay(name);

            string prefix = "All";
            if (parsedDay >= 0)
                prefix = parsedDay.ToString();

            return prefix + "_" + name;
        }

        public bool IsValid { get; private set; }
        public Elements InvalidElements { get; private set; }



        /// <summary>
        /// Fills this Conversation's elements and choices.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="data"></param>
        public void Fill(ConversationRange range, TSVData data)
        {
            fileName = range.fileName;

            // Loop through the data, skipping the Conversation and End markers
            for (int i = range.start + 1; i < range.end - 1; i++)
            {
                string marker = data.CharacterIDColumn[i].ToLower();
                if (string.IsNullOrEmpty(marker) && Enum.TryParse(data.LineIDColumn[i], out LineID id))
                    elements.Add(new DialogueElement(id));
                else if (marker == IfMarker)
                    elements.Add(new IfElement(data.TextColumn[i]));
                else if (marker == ElifMarker || marker == ElseIfMarker)
                    elements.Add(new ElifElement(data.TextColumn[i]));
                else if (marker == ElseMarker)
                    elements.Add(new ElseElement());
                else if (marker == EndIfMarker || marker == End_IfMarker)
                    elements.Add(new EndIfElement());
                else if (marker == ChoiceMarker && Enum.TryParse(data.TextColumn[i + 1], out LineID prompt))
                    choices.Add(new RawChoice(prompt, data.TextColumn[i]));
                //else if (marker == GotoMarker)
                // TODO: Implement Goto later
            }
        }


        public void Validate(List<string> conversationNames)
        {
            InvalidElements = GetInvalidElements(conversationNames);
            IsValid = InvalidElements == Elements.None;
        }

        private Elements GetInvalidElements(List<string> conversationNames)
        {
            Elements invalidElements = Elements.None;
            if (!IsNameValid()) invalidElements |= Elements.InvalidName;

            for (int i = 0; i < elements.Count; i++)
            {
                IConversationElement element = elements[i];

                if (element is IfElement && !IsIfValid(i))
                    invalidElements |= Elements.NoEndIf;
                else if ((element is ElifElement || element is ElseElement) && !IsElifOrElseValid(i))
                    invalidElements |= Elements.NoStartingIf;
                else if (element is GotoElement _goto && _goto.conversation == null)
                    invalidElements |= Elements.InvalidGotoTarget;
            }

            for (int i = 0; i < choices.Count; i++)
            {
                if (!conversationNames.Contains(choices[i].nextConversation))
                    invalidElements |= Elements.InvalidChoiceTarget;
            }

            return invalidElements;
        }

        public string GetInvalidElementsString()
        {
            if (IsNameValid())
                return $"Conversation '{name}' (day={day}): {InvalidElements}";
            else
                return $"Conversation (invalid name, day={day}, {elements.Count} elements): {InvalidElements}";
            //return $"Raw Line with ID {id} is invalid. Invalid elements: {InvalidElements}.";
        }


        public bool IsNameValid() => !string.IsNullOrEmpty(name);
        bool IsIfValid(int ifIndex)
        {
            // Start at the next element
            for (int i = ifIndex + 1; i < elements.Count; i++)
            {
                IConversationElement element = elements[i];
                // There is an EndIf element - we are good
                if (element is EndIfElement)
                    return true;
                // Ruh roh - found another If
                else if (element is IfElement)
                    return false;
            }

            // No EndIf found
            return false;
        }

        bool IsElifOrElseValid(int elifOrElseIndex)
        {
            for (int i = elifOrElseIndex - 1; i >= 0; i--)
            {
                IConversationElement element = elements[i];

                // There is a starting If element - we are good
                if (element is IfElement)
                    return true;

                // Uh oh... there is no starting If
                else if (element is EndIfElement)
                    return false;
                // Two elses in a row or an Else before an ElIf (big nono)
                else if (element is ElseElement)
                    return false;
            }

            return false;
        }


        [Flags]
        public enum Elements
        {
            None = 0,
            NoStartingIf = 1 << 0,
            NoEndIf = 1 << 1,
            InvalidChoiceTarget = 1 << 2,
            InvalidName = 1 << 3,
            InvalidGotoTarget = 1 << 4,
        }
    }

    public class RawChoice
    {
        public LineID prompt;
        public string nextConversation;

        public RawChoice(LineID prompt, string nextConversation)
        {
            this.prompt = prompt;
            this.nextConversation = nextConversation.Trim();
        }
    }
}
