using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;

public class KeyGraphics : MonoBehaviour
{
    void Start()
    {
        // Enable the key if we have the car or if this is the tutorial
        gameObject.SetActive(Cabin.HasCar || (Day.DayNumber == Day.TutorialDay && Day.Stage == Stage.SpeakingWithParty));
    }
}