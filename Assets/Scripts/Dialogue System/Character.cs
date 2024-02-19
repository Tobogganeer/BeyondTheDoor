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
        public CharacterID ID { get; private set; }
        public CharacterStatus Status { get; set; }
    }

    public enum CharacterStatus
    {
        NotMet,
        AtDoor,
        LeftOutside,
        PartyMember,
        Scavenging,
        DeadWhileScavenging,
        KilledByBear,
        Kidnapped,
        DeadFromIllness,
        Killed,
        DeadOnWayToBorder,
        AliveAtBorder
    }
}
