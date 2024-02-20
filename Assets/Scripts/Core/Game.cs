using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondTheDoor
{
    public class Game : MonoBehaviour
    {
        private static Game instance;
        private void Awake()
        {
            instance = this;
        }

        public static void Begin()
        {
            throw new NotImplementedException();
        }

        [SerializeField] private List<CharacterID> characterArrivalOrder;

        public static List<CharacterID> CharacterArrivalOrder => instance.characterArrivalOrder;
    }
}
