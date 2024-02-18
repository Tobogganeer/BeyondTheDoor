using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToBOE.Dialogue;
using ToBOE.Dialogue.UI;
using System;

/// <summary>
/// Shows the desired usage of the dialogue system
/// </summary>
public class DesiredUsage : MonoBehaviour
{
    private void Start()
    {
        /*
        IEnumerable<Line> jessicaLines = LineSearch.FilterCharacter(Line.All.Values, CharacterID.Jessica);
        foreach (Line line in jessicaLines)
        {
            Debug.Log(line);
        }
        */
        //Line.startConvo.Then(Line.convo2).Then(Line.askQuestion);
        //Line.askQuestion.ThenChoice(Choice.Line(Line.sure, Line.soundsGood), Choice.Action(Line.shoot, ShootLover));

        /*
        Line.bob_start_joke.Then(Line.jessica_reply1).Then(Line.bob_joke_question).Then(Line.jessica_reply2).
            Then(Line.bob_punchline);
        Line.bob_punchline.Then(Line.jessica_silent);
        Line.jessica_silent.ThenChoice(Choice.Line(Line.player_no, null), Choice.Action(Line.player_yes, KillBob));

        Line.bob_start_joke.Open();
        */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!DialogueGUI.HasLine)
                Line.bob_start_joke.Open();
            else
                DialogueGUI.Next();
        }
    }

    /*
    void KillBob(Line l)
    {

    }
    */


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
