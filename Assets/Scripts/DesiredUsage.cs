using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeyondTheDoor;
using BeyondTheDoor.UI;
using UnityEngine.InputSystem;
using System;
using BeyondTheDoor.SaveSystem;

/// <summary>
/// Shows the desired usage of the dialogue system
/// </summary>
public class DesiredUsage : MonoBehaviour
{
    public Conversation convo;

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

        //Line.bob_start_joke.Then(Line.jessica_reply1).Then(Line.bob_joke_question).Then(Line.jessica_reply2).
        //    Then(Line.bob_punchline);
        //Line.bob_punchline.Then(Line.jessica_silent).Then(Line.jessica_player_question);
        //Line.jessica_player_question.ThenChoice(Choice.Line(Line.player_no, Line.jessica_cant_kill_bob), Choice.Action(Line.player_yes, KillBob));

        //Line.bob_start_joke.Open();

        SaveState state = SaveSystem.Load(1);
        state.Load();
        Game.Begin();
        // Do stuff...
        Day.Advance();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!DialogueGUI.HasLine)
                convo.Start();
        }
    }

    
    //void KillBob(Line l)
    //{
    //
    //}
    


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
