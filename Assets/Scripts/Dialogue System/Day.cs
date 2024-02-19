using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToBOE.Dialogue
{
    [Serializable]
    public class Day
    {
        [SerializeField, Range(0, 8)] private int number;
        [SerializeField] private CharacterID arrivingCharacter;

        public int Number => number;
        public CharacterID ArrivingCharacter => arrivingCharacter;
    }
}
