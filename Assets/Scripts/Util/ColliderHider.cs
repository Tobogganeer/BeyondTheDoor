using BeyondTheDoor.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderHider : MonoBehaviour
{
    Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    void Update()
    {
        coll.enabled = !DialogueGUI.IsOpen;
    }
}
