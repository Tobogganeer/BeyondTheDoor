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
        public CharacterStatus LastStatus => HistoryEvents.Count > 0 ? HistoryEvents[HistoryEvents.Count - 1].OldStatus : Status;
        /// <summary>
        /// A list of events that have happened to this character
        /// </summary>
        public List<CharacterHistoryEvent> HistoryEvents { get; set; }
        /*
        /// <summary>
        /// The first thing this Character says at the door
        /// </summary>
        public IDialogueElement Introduction { get; set; }
        */

        public bool NameAlwaysKnown { get; set; } = true;


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
        public bool JustReturnedFromScavenging => LastStatus == CharacterStatus.ScavengingWithShotgun ||
            LastStatus == CharacterStatus.ScavengingDefenseless;


        // ============ Dialogue Events ============
        /// <summary>
        /// Called when this character arrives at the door.
        /// </summary>
        /// <remarks>Start dialogue</remarks>
        public event Action<Character> ArrivingAtDoor;
        /// <summary>
        /// Called when the player clicks on this character while another character is at the door.
        /// </summary>
        /// <remarks>Start dialogue</remarks>
        public event Action<Character> OtherCharacterArrivingAtDoor;
        /// <summary>
        /// Called when the player clicks on this character during the day.
        /// </summary>
        /// <remarks>Start dialogue</remarks>
        public event Action<Character> SpokenTo;
        /// <summary>
        /// Called when the player clicks on this character during the scavenging stage.
        /// </summary>
        /// <remarks>Add/remove from the scavenge party (internal use)</remarks>
        public event Action<Character> ClickedOnDuringScavengeStage;
        /// <summary>
        /// Called when the player wants to send this character scavenging.
        /// </summary>
        /// <remarks>Start dialogue</remarks>
        public event Action<Character> AddedToScavengeParty;
        /// <summary>
        /// Called when the player decides to not send this character scavenging.
        /// </summary>
        /// <remarks>Start dialogue</remarks>
        public event Action<Character> RemovedFromScavengeParty;
        /*
        /// <summary>
        /// Called when the player clicks the button to start the scavenge.
        /// </summary>
        /// <remarks>Call SendToScavenge_With/NoShotgun callback</remarks>
        public event Action<Character> AboutToBeSentScavenging;
        */
        /// <summary>
        /// Called when the player sends this character out to scavenge. Passes the character and whether or not they have the shotgun.
        /// </summary>
        /// <remarks>Call AdvanceStage callback</remarks>
        public event Action<Character, bool> SentToScavenge;
        /// <summary>
        /// Called when the player clicks on this character during the overcrowding stage.
        /// </summary>
        /// <remarks>Start dialogue and call KickOut callback</remarks>
        public event Action<Character> ClickedOnDuringOvercrowding;


        // ============ Constants ============
        static readonly string UnknownCharacterName = "Unknown";
        

        /// <summary>
        /// The character that is currently being talked to.
        /// </summary>
        public static Character Current { get; set; }


        public Character(CharacterID id)
        {
            ID = id;
            Reset();
        }

        // We can't just recreate all the characters because they have old references in Character.___
        private void Reset()
        {
            Name = ID.ToString(); // Can be changed later
            Status = CharacterStatus.NotMet;
            HistoryEvents = new List<CharacterHistoryEvent>();

            ResetEvents();
        }

        private void ResetEvents()
        {
            ArrivingAtDoor = null;
            OtherCharacterArrivingAtDoor = null;
            SpokenTo = null;
            ClickedOnDuringScavengeStage = null;
            AddedToScavengeParty = null;
            RemovedFromScavengeParty = null;
            //AboutToBeSentScavenging = null;
            SentToScavenge = null;
            ClickedOnDuringOvercrowding = null;
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
            //if (Introduction != null)
            //    Introduction.Open();
            //else
            //    throw new NullReferenceException($"Character ({ID}) has no Introduction! Please set it through code.");
        }

        /// <summary>
        /// Call this when the player wants to speak to this character (during any stage)
        /// </summary>
        public void OnSelected()
        {
            switch (Day.Stage)
            {
                case Stage.MorningSupplies:
                    break;
                case Stage.SpeakingWithParty:
                    Current = this;
                    SpokenTo?.Invoke(this);
                    break;
                case Stage.SendingScavengers:
                    Current = this;
                    ClickedOnDuringScavengeStage?.Invoke(this);
                    break;
                case Stage.FixingOvercrowding:
                    Current = this;
                    ClickedOnDuringOvercrowding?.Invoke(this);
                    break;
                case Stage.DealingWithArrival:
                    Current = this;
                    OtherCharacterArrivingAtDoor?.Invoke(this);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the name of the character if they have been introduced.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentName()
        {
            return (HasBeenIntroduced || NameAlwaysKnown) ? Name : UnknownCharacterName;
        }


        /// <summary>
        /// Resets the data of all characters to their default state.
        /// </summary>
        public static void ResetAll()
        {
            foreach (Character c in All.Values)
                c.Reset();
        }


        public void Invoke_SentToScavenge(bool shotgun) => SentToScavenge?.Invoke(this, shotgun);
        public void Invoke_AddedToScavengeParty() => AddedToScavengeParty?.Invoke(this);
        public void Invoke_RemovedFromScavengeParty() => RemovedFromScavengeParty?.Invoke(this);
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
        KickedOut,
        Left, // Used for Dad and Hal
        LeftWithShotgun, // Used for Dad
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
