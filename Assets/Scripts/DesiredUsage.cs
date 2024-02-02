using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToBOE.Dialogue;
using System;

/// <summary>
/// Shows the desired usage of the dialogue system
/// </summary>
public class DesiredUsage : MonoBehaviour
{
    private void Start()
    {
        //Line.startConvo.Then(Line.convo2).Then(Line.askQuestion);
        //Line.askQuestion.ThenChoice(Choice.Line(Line.sure, Line.soundsGood), Choice.Action(Line.shoot, ShootLover));


        //Line.startConvo.Open();
    }


    /*
    class Choice
    {
        Line spoken;
        Line following;
        Action<Line> onChosen;

        static Choice Line(Line spoken, Line followup) { } // Opens 'following' when chosen
        static Choice Action(Line spoken, Action<Line> onChosen) { } // Calls the action when chosen
        static Choice LineAndAction(Line spoken, Line followup, Action<Line> onChosen) { } // Does both
    }
    */
}
