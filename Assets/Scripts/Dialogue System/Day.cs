using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ToBOE.Dialogue
{
    /// <summary>
    /// Represents the course of a day.
    /// </summary>
    [Serializable]
    public class Day
    {
        [SerializeField, Range(0, 8)] private int number;
        [SerializeField] private CharacterID arrivingCharacter;

        public int Number => number;
        public CharacterID ArrivingCharacter => arrivingCharacter;
        public Stage Stage { get; set; }
    }

    /// <summary>
    /// Represents a stage of the day.
    /// </summary>
    public enum Stage
    {
        SpeakingWithParty,
        SendingScavengers,
        FixingOvercrowding,
        RadioLoreTime,
        DealingWithArrival
    }
}
