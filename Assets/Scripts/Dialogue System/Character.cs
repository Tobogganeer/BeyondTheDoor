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


        // ============ Constants ============
        static readonly string UnknownCharacterName = "(Unknown)";


        public Character(CharacterID id)
        {
            ID = id;
            Name = id.ToString(); // Can be changed later
            Status = CharacterStatus.NotMet;
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
