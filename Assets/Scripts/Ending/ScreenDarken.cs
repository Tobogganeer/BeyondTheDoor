using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenDarken : MonoBehaviour
{
    [SerializeField] private Image rect;
    [SerializeField] private float timeToDarken;
    private float timer = 0f; // Timer to keep track of elapsed time

    void Start()
    {
        
        //hehe
    }

    void Update()
    {
        CountUptimer();
    }
    private void CountUptimer()
    {
        Color color = rect.color;
        timer += Time.deltaTime;
        color.a = timer/ timeToDarken;
        //Debug.Log(timer/3);
        rect.color = color;
    }
}
