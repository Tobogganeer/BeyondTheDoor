using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BeyondTheDoor
{
    /// <summary>
    /// Represents the state of the current day.
    /// </summary>
    public static class Day
    {
        // Not sure why I'm making these constants, but I am
        public const int FirstDay = 0;
        public const int LastDay = 8;

        public static int DayNumber { get; private set; }
        public static CharacterID ArrivingCharacter { get; private set; }
        public static Stage Stage { get; set; }

        public static void StartDay(int dayNumber)
        {
            if (dayNumber < FirstDay || dayNumber > LastDay)
                throw new ArgumentException($"Tried to load day {dayNumber}?", nameof(dayNumber));

            DayNumber = dayNumber;
            ArrivingCharacter = Game.CharacterArrivalOrder[dayNumber];
            Stage = Stage.SpeakingWithParty;
        }
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
