using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //dasd

    private void getDay()
    {
        dayCounter.text = "Day:" + Day.DayNumber.ToString() + "\n" + Day.Stage.ToString() ;
    }

    // Update is called once per frame
    void Update()
    {
        getDay();
    }
}
