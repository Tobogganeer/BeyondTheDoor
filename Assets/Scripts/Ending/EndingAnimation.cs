using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class EndingAnimation : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<string> phrases;
    [SerializeField] private float timeBetweenTransitions;

    IEnumerator Start()
    {
        yield return WriteList(timeBetweenTransitions, "6 people", "Hard decisions were made", "People were sent to their death", "Only 3 left out unharmed", "We", "SURVIVED", "A new dawn rises", "A new life starts");
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && !Cabin.HasCar)
        {
            yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", " I blinked and now I’m dead", "Is it all my fault, the raiders?", "everyone the took, was it my fault?", "or it was destined to happen?", "We were at war", "How could’ve done better?", "I guess theres no more chances for me", "or those who I met in the last days", "I’m sorry", "I’m truly sorry");
        }
        if (Character.Player.Status == CharacterStatus.KilledByBear)
        {
            yield return WriteList(timeBetweenTransitions, "What the fuck was going on my head", "When I let a bear get inside my house","I’m dead now","What makes me even sadder", "is that I’m just a number now","like those who where caught between conflicting ideas","my death is irrelevant", "as it will all be forgotten in the future","I don’t think I can judge them to be honest", "it is in the human nature to not care about stuff", "that doesn’t affect them");
        }
        {
            yield return WriteList(timeBetweenTransitions, "I was feeling very tired", "I felt my leg tremble as I walked", " I blinked and now I’m dead", "Is it all my fault, the raiders?", "everyone the took, was it my fault?", "or it was destined to happen?", "We were at war", "How could’ve done better?", "I guess theres no more chances for me", "or those who I met in the last days", "I’m sorry", "I’m truly sorry");
        }
        if(Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Bob.Status == CharacterStatus.AliveAtBorder && Character.Sal.Status == CharacterStatus.AliveAtBorder && Character.Hal.Status == CharacterStatus.AliveAtBorder)
        {
            //implement later
        }
        if (Character.Player.Status == CharacterStatus.AliveAtBorder && Character.Bob.Status == CharacterStatus.AliveAtBorder && Character.Jessica.Status == CharacterStatus.AliveAtBorder)
        {
            yield return WriteList(timeBetweenTransitions, "I thought Bob was going to break his promise of Jessica", "taking care of Jessica even after reaching the border”, “I was wrong”, “Very wrong” “They are together now,” “I guess that”,” being stuck near someone”, “you dislike for such a long time can change their views”, “ I guess it was worth it in the end" );
        }
    }
    IEnumerator WriteList(float delay, params string[] textToDisplay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        for (int i = 0; i < textToDisplay.Length; i++)
        {
            text.text = textToDisplay[i];
            yield return wait;
        }
    }
}

