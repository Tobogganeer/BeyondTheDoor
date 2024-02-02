using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBOE.Dialogue
{
    public class DialogueGUI : MonoBehaviour
    {
        public static void OpenLine(Line line)
        {
            // TODO: Display stuff

            // TODO: Call only when the line is actually done displaying
            line.OnLineClosing();
        }

        public static void OpenChoices(ChoiceCollection choices)
        {
            // TODO: Display stuff and make the choice

            // TODO: Call only when the choice is actually made
            choices.Choices[0].OnChoiceChosen();
        }
    }
}
