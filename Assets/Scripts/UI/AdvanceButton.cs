using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvanceButton : MonoBehaviour
{
    public ConversationCallback advance;

    [Space]
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(() => advance.Invoke(null, 0));
    }

    private void Update()
    {
        // Turn it on if we can advance
        button.gameObject.SetActive(ShouldButtonBeOn());
    }

    bool ShouldButtonBeOn()
    {
        bool isTutorial = Day.DayNumber == Day.TutorialDay;
        return !isTutorial && Game.CanAdvance() && Day.Stage != Stage.DealingWithArrival;
    }
}
