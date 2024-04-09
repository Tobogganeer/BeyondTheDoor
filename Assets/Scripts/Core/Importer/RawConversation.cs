using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor.Importer
{
    public class RawConversationData
    {
        public string name;
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

        int FindDay(string name)
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

        public bool IsValid { get; private set; }
        public Elements InvalidElements { get; private set; }


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
                return $"Conversation '{name}': {InvalidElements}";
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
