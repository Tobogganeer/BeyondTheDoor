using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Open()
    {
        canvas.enabled = true;
    }

    public void Close()
    {
        canvas.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
