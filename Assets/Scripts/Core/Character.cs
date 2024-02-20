using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    /// <summary>
    /// Respresents a character.
    /// </summary>
    [Serializable]
    public partial class Character
    {
        // ============ Variables ============
        public CharacterID ID { get; private set; }
        public string Name { get; set; }
        public CharacterStatus Status { get; set; }
        public List<CharacterHistoryEvent> HistoryEvents { get; set; }


        // ============ Useful Properties ============
        /// <summary>
        /// Has this character been to the door?
        /// </summary>
        public bool HasBeenEncountered => Status >= CharacterStatus.AtDoor;
        /// <summary>
        /// Has this character been inside the cabin?
        /// </summary>
        public bool HasBeenIntroduced => Status >= CharacterStatus.PartyMember;
        /// <summary>
        /// Is this character currently dead?
        /// </summary>
        public bool IsDead => Status >= CharacterStatus.DeadWhileScavenging;
        /// <summary>
        /// Is this character currently out scavenging?
        /// </summary>
        public bool IsScavenging => Status == CharacterStatus.ScavengingWithShotgun
            || Status == CharacterStatus.ScavengingDefenseless;
        /// <summary>
        /// Has the player chosen to let this character in or leave them out?
        /// </summary>
        public bool DoorDecisionMade => Status > CharacterStatus.AtDoor;


        // ============ Dialogue Events ============
        /// <summary>
        /// Called when this character arrives at the door (start dialogue).
        /// </summary>
        public event Action<Character> OnArrivalAtDoor;
        /// <summary>
        /// Called when the player clicks on this character during the day (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenTo;

        #region Individual Days
        /// <summary>
        /// Called when the player clicks on this character during day 0 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay0;
        /// <summary>
        /// Called when the player clicks on this character during day 1 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay1;
        /// <summary>
        /// Called when the player clicks on this character during day 2 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay2;
        /// <summary>
        /// Called when the player clicks on this character during day 3 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay3;
        /// <summary>
        /// Called when the player clicks on this character during day 4 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay4;
        /// <summary>
        /// Called when the player clicks on this character during day 5 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay5;
        /// <summary>
        /// Called when the player clicks on this character during day 6 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay6;
        /// <summary>
        /// Called when the player clicks on this character during day 7 (start dialogue).
        /// </summary>
        public event Action<Character> OnSpokenToDay7;
        #endregion


        // ============ Constants ============
        static readonly string UnknownCharacterName = "(Unknown)";


        public Character(CharacterID id)
        {
            ID = id;
            Name = id.ToString(); // Can be changed later
            Status = CharacterStatus.NotMet;
            HistoryEvents = new List<CharacterHistoryEvent>();
        }



        /// <summary>
        /// Call this when the player wants to speak to this character
        /// </summary>
        public void OnSelected()
        {
            OnSpokenTo?.Invoke(this);
            InvokeIndividualDayOnSpokenTo();
        }

        private void InvokeIndividualDayOnSpokenTo()
        {
            // Yuck
            // I reaaally want to make an array of these but I can't :(
            switch (Day.DayNumber)
            {
                case 0: OnSpokenToDay0?.Invoke(this); break;
                case 1: OnSpokenToDay1?.Invoke(this); break;
                case 2: OnSpokenToDay2?.Invoke(this); break;
                case 3: OnSpokenToDay3?.Invoke(this); break;
                case 4: OnSpokenToDay4?.Invoke(this); break;
                case 5: OnSpokenToDay5?.Invoke(this); break;
                case 6: OnSpokenToDay6?.Invoke(this); break;
                case 7: OnSpokenToDay7?.Invoke(this); break;
                default: break;
            }
        }

        /// <summary>
        /// Gets the name of the character if they have been introduced.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentName()
        {
            return HasBeenIntroduced ? Name : UnknownCharacterName;
        }
    }

    public enum CharacterStatus
    {
        NotMet,
        AtDoor,
        LeftOutside,
        PartyMember,
        ScavengingDefenseless,
        ScavengingWithShotgun,
        AliveAtBorder,
        DeadWhileScavenging,
        KilledByBear,
        Kidnapped,
        DeadFromIllness,
        Killed,
        DeadOnWayToBorder,
    }

    /// <summary>
    /// Represents an event happening to a character.
    /// </summary>
    public class CharacterHistoryEvent
    {
        /// <summary>
        /// What was this character?
        /// </summary>
        public CharacterStatus OldStatus { get; private set; }
        /// <summary>
        /// What is this character now?
        /// </summary>
        public CharacterStatus NewStatus { get; private set; }
        /// <summary>
        /// What day did this happen on?
        /// </summary>
        public int Day { get; private set; }
        /// <summary>
        /// What stage of the day did it happen on?
        /// </summary>
        public Stage Stage { get; private set; }
        
        /// <summary>
        /// Creates a history event with the passed arguments.
        /// </summary>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        /// <param name="day"></param>
        /// <param name="stage"></param>
        public CharacterHistoryEvent(CharacterStatus oldStatus, CharacterStatus newStatus, int day, Stage stage)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Day = day;
            Stage = stage;
        }
        
        /// <summary>
        /// Creates a history event with with the passed statuses, using the current day and stage.
        /// </summary>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        public CharacterHistoryEvent(CharacterStatus oldStatus, CharacterStatus newStatus)
            : this(oldStatus, newStatus, BeyondTheDoor.Day.DayNumber, BeyondTheDoor.Day.Stage) { }
    }
}
