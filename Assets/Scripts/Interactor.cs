using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BeyondTheDoor;
using BeyondTheDoor.UI;

public class Interactor : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Check if we click while not speaking already
        if (!DialogueGUI.IsOpen && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.value);
            // Check if we clicked on something interactable
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnClicked();
            }
        }
    }
}

public interface IInteractable
{
    void OnClicked();
}
