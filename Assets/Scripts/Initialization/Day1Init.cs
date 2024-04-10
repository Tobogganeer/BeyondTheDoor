using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Init : DayBehaviour
{
    public Conversation wakeUpDay1;

    protected override int GetDay() => 1;

    protected override void Initialize()
    {
       
        if (Stage == Stage.MorningSupplies)
        {

            wakeUpDay1.Start();
        }
 
    }



}
