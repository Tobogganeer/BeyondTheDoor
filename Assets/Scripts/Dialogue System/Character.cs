using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBOE.Dialogue
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


        // ============ Useful Properties ============
        /// <summary>
        /// Has this character been to the door?
        /// </summary>
        public bool Encountered => Status >= CharacterStatus.AtDoor;
        /// <summary>
        /// Has this character been inside the cabin?
        /// </summary>
        public bool Introduced => Status >= CharacterStatus.PartyMember;


        // ============ Dialogue Events ============
        /// <summary>
        /// Called when this character arrives at the door (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnArrivalAtDoor;
        /// <summary>
        /// Called when the player clicks on this character during the day (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenTo;

        #region Individual Days
        /// <summary>
        /// Called when the player clicks on this character during day 0 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay0;
        /// <summary>
        /// Called when the player clicks on this character during day 1 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay1;
        /// <summary>
        /// Called when the player clicks on this character during day 2 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay2;
        /// <summary>
        /// Called when the player clicks on this character during day 3 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay3;
        /// <summary>
        /// Called when the player clicks on this character during day 4 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay4;
        /// <summary>
        /// Called when the player clicks on this character during day 5 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay5;
        /// <summary>
        /// Called when the player clicks on this character during day 6 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay6;
        /// <summary>
        /// Called when the player clicks on this character during day 7 (start dialogue).
        /// </summary>
        public event Action<Character, Day> OnSpokenToDay7;
        #endregion


        // ============ Constants ============
        static readonly string UnknownCharacterName = "(Unknown)";


        public Character(CharacterID id)
        {
            ID = id;
            Name = id.ToString(); // Can be changed later
            Status = CharacterStatus.NotMet;
        }



        /// <summary>
        /// Call this when the player wants to speak to his character
        /// </summary>
        public void OnSelected()
        {
            // TODO: Activate callbacks using current Day
        }

        /// <summary>
        /// Gets the name of the character if they have been introduced.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentName()
        {
            return Introduced ? Name : UnknownCharacterName;
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
        DeadWhileScavenging,
        KilledByBear,
        Kidnapped,
        DeadFromIllness,
        Killed,
        DeadOnWayToBorder,
        AliveAtBorder
    }
}
