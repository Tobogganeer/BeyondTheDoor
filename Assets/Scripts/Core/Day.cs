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
        public const int TutorialDay = 0;
        public const int FirstDay = 1;
        public const int LastDay = 7;
        public const int BorderDay = 8;

        public static int DayNumber { get; internal set; }
        //public static CharacterID ArrivingCharacter { get; private set; }
        public static Stage Stage { get; set; }

        public static CharacterID GetArrivingCharacter()
        {
            return Game.CharacterArrivalOrder[DayNumber];
        }

        public static void Advance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the given day.
        /// </summary>
        /// <param name="dayNumber"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void StartDay(int dayNumber)
        {
            if (dayNumber < TutorialDay || dayNumber > BorderDay)
                throw new ArgumentException($"Tried to load day {dayNumber}?", nameof(dayNumber));

            DayNumber = dayNumber;
            //ArrivingCharacter = Game.CharacterArrivalOrder[dayNumber];
            Stage = Stage.SpeakingWithParty;
        }

        /// <summary>
        /// Resets the current day, or resets all the way to Day 1 if <paramref name="resetToDay1"/> is true.
        /// </summary>
        /// <param name="resetToDay1"></param>
        public static void Reset(bool resetToDay1 = false)
        {
            StartDay(resetToDay1 ? FirstDay : DayNumber);
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
