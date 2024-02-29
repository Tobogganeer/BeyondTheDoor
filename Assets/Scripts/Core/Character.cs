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
        /// <summary>
        /// This Character's ID
        /// </summary>
        public CharacterID ID { get; private set; }
        /// <summary>
        /// This Character's proper name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The current status of this Character
        /// </summary>
        public CharacterStatus Status { get; private set; }
        /// <summary>
        /// A list of events that have happened to this character
        /// </summary>
        public List<CharacterHistoryEvent> HistoryEvents { get; set; }
        /// <summary>
        /// The first thing this Character says at the door
        /// </summary>
        public IDialogueElement Introduction { get; set; }


        // ============ Useful Properties ============
        /// <summary>
        /// Has this character been to the door?
        /// </summary>
        public bool HasBeenEncountered => Status >= CharacterStatus.AtDoor;
        /// <summary>
        /// Has this character been inside the cabin?
        /// </summary>
        public bool HasBeenIntroduced => Status >= CharacterStatus.InsideCabin;
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
        public bool InsideCabin => Status == CharacterStatus.InsideCabin;


        // ============ Dialogue Events ============
        /// <summary>
        /// Called when this character arrives at the door (start dialogue).
        /// </summary>
        public event Action<Character> ArrivingAtDoor;
        /// <summary>
        ///Called when the player clicks on this character while another character is at the door (start dialogue).
        /// </summary>
        public event Action<Character> OtherCharacterArrivingAtDoor;
        /// <summary>
        /// Called when the player clicks on this character during the day (start dialogue).
        /// </summary>
        public event Action<Character> SpokenTo;

        #region Individual Days
        /// <summary>
        /// Called when the player clicks on this character during day 0 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay0;
        /// <summary>
        /// Called when the player clicks on this character during day 1 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay1;
        /// <summary>
        /// Called when the player clicks on this character during day 2 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay2;
        /// <summary>
        /// Called when the player clicks on this character during day 3 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay3;
        /// <summary>
        /// Called when the player clicks on this character during day 4 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay4;
        /// <summary>
        /// Called when the player clicks on this character during day 5 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay5;
        /// <summary>
        /// Called when the player clicks on this character during day 6 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay6;
        /// <summary>
        /// Called when the player clicks on this character during day 7 (start dialogue).
        /// </summary>
        public event Action<Character> SpokenToDay7;
        #endregion

        /// <summary>
        /// Called when the player clicks on this character during the scavenging stage (start dialogue).
        /// </summary>
        public event Action<Character> SendingToScavenge;
        /// <summary>
        /// Called when the player clicks on this character during the overcrowding stage (start dialogue).
        /// </summary>
        public event Action<Character> TryingToKickOut;


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
        /// Changes this Character's status to <paramref name="newStatus"/> and records it in our <seealso cref="HistoryEvents"/>.
        /// </summary>
        /// <param name="newStatus"></param>
        public void ChangeStatus(CharacterStatus newStatus, bool writeHistory = true)
        {
            if (writeHistory && newStatus != Status)
            {
                HistoryEvents.Add(new CharacterHistoryEvent(Status, newStatus));
                Status = newStatus;
            }
            else
            {
                Status = newStatus;
            }
        }

        /// <summary>
        /// Call this when the character arrives at the door.
        /// </summary>
        public void OnArrivingAtDoor()
        {
            ArrivingAtDoor?.Invoke(this);
            if (Introduction != null)
                Introduction.Open();
            else
                throw new NullReferenceException($"Character ({ID}) has no Introduction! Please set it through code.");
        }

        /// <summary>
        /// Call this when the player wants to speak to this character (during any stage)
        /// </summary>
        public void OnSelected()
        {
            switch (Day.Stage)
            {
                case Stage.SpeakingWithParty:
                    SpokenTo?.Invoke(this);
                    InvokeIndividualDayOnSpokenTo();
                    break;
                case Stage.SendingScavengers:
                    SendingToScavenge?.Invoke(this);
                    break;
                case Stage.FixingOvercrowding:
                    TryingToKickOut?.Invoke(this);
                    break;
                case Stage.RadioLoreTime:
                    break;
                case Stage.DealingWithArrival:
                    OtherCharacterArrivingAtDoor?.Invoke(this);
                    break;
                default:
                    break;
            }
        }

        private void InvokeIndividualDayOnSpokenTo()
        {
            // Yuck
            // I reaaally want to make an array of these but I can't :(
            switch (Day.DayNumber)
            {
                case 0: SpokenToDay0?.Invoke(this); break;
                case 1: SpokenToDay1?.Invoke(this); break;
                case 2: SpokenToDay2?.Invoke(this); break;
                case 3: SpokenToDay3?.Invoke(this); break;
                case 4: SpokenToDay4?.Invoke(this); break;
                case 5: SpokenToDay5?.Invoke(this); break;
                case 6: SpokenToDay6?.Invoke(this); break;
                case 7: SpokenToDay7?.Invoke(this); break;
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


        /// <summary>
        /// Resets the data of all characters to their default state.
        /// </summary>
        public static void ResetAll()
        {
            Dictionary<CharacterID, Character> newChars = new Dictionary<CharacterID, Character>();

            // Recreate them all
            foreach (Character c in All.Values)
                newChars.Add(c.ID, new Character(c.ID));

            All = newChars;
        }
    }

    public enum CharacterStatus
    {
        NotMet,
        AtDoor,
        LeftOutside,
        InsideCabin,
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
