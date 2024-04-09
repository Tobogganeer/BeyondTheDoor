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
            this.name = name;
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

        [System.Flags]
        public enum Elements
        {
            None = 0,
            NoStartingIf = 1 << 0,
            NoEndIf = 1 << 1,
            InvalidChoiceTarget = 1 << 2,
        }
    }

    public class RawChoice
    {
        public LineID prompt;
        public string nextConversation;

        public RawChoice(LineID prompt, string nextConversation)
        {
            this.prompt = prompt;
            this.nextConversation = nextConversation;
        }
    }
}
