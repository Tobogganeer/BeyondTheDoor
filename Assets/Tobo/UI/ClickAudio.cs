using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private static float lastHoverAudioTime;
    private static float lastClickAudioTime;
    private const float CLICK_MIN_DELAY = 0.06f;
    private const float HOVER_MIN_DELAY = 0.035f;

    public static void Hover()
    {
        if (lastHoverAudioTime - Time.time < -HOVER_MIN_DELAY)
        {
            AudioManager.PlayLocal2D("uihover");
            lastHoverAudioTime = Time.time;
        }
    }

    public static void Click()
    {
        if (lastClickAudioTime - Time.time < -CLICK_MIN_DELAY)
        {
            AudioManager.PlayLocal2D("uiclick");
            lastClickAudioTime = Time.time;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }
}
