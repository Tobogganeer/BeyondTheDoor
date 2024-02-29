using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public static class Cabin
    {
        private const int MaxPartyMembers = 2;

        public static bool HasShotgun { get; set; } = true;
        public static bool HasCar { get; set; } = false;



        public static int NumCurrentPartyMembers()
        {
            int num = 0;
            foreach (Character character in Character.All.Values)
            {
                if (character.InsideCabin)
                    num++;
            }

            return num;
        }
        public static List<Character> CurrentPartyMembers()
        {
            List<Character> chars = new List<Character>();
            foreach (Character c in Character.All.Values)
            {
                if (c.InsideCabin)
                    chars.Add(c);
            }

            return chars;
        }
        public static bool IsOvercrowded()
        {
            int numMembers = NumCurrentPartyMembers();
            if (Character.Hal.InsideCabin && Character.Sal.InsideCabin)
                numMembers--; // The siblings count as 1 character together
            return numMembers > MaxPartyMembers;
        }


        public static void Reset()
        {
            HasShotgun = true;
            HasCar = true;
        }
    }
}
