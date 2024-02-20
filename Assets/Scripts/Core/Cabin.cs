using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public static class Cabin
    {
        public static bool HasShotgun { get; set; } = true;
        public static bool HasCar { get; set; } = false;
        public static List<Character> CurrentCharacters => GetCurrentCharacters();

        public static List<Character> GetCurrentCharacters()
        {
            List<Character> chars = new List<Character>();
            foreach (Character c in Character.All.Values)
            {
                if (c.InsideCabin)
                    chars.Add(c);
            }

            return chars;
        }

        public static void Reset()
        {
            HasShotgun = true;
            HasCar = true;
        }
    }
}
