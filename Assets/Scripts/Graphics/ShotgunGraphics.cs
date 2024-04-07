using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunGraphics : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(BeyondTheDoor.Cabin.HasShotgun);
    }
}
