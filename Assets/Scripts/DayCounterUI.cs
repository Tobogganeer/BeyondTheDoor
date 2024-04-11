using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayCounter;
    void Start()
    {
        dayCounter.text = "Day " + Day.DayNumber.ToString() + " - " + Day.Stage.ToTimeString();
    }
}
