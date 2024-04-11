using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    void Start()
    {
        if (Day.DayNumber != Day.TutorialDay)
            Destroy(gameObject);
    }
}
