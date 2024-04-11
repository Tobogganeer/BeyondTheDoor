using BeyondTheDoor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunModelManager : MonoBehaviour
{
    [SerializeField] private GameObject shotgun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        check();
    }

    private void check()
    {
        if(!Cabin.HasShotgun)
        {
            shotgun.SetActive(false);
        }
    }
}
