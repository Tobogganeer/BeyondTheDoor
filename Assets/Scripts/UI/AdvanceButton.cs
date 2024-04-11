using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AdvanceButton : MonoBehaviour
{
    public ConversationCallback advance;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => advance.Invoke(null, 0));
    }
}
