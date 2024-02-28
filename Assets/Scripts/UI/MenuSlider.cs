using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_Text valueText;
    public float textValueMultiplier = 100f; // Value is usually 0-1

    public float value
    {
        get => slider.value;
        set
        {
            slider.value = value;
            UpdateText();
        }
    }
    public event Action<float> ValueChanged;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        ValueChanged?.Invoke(value);
        UpdateText();
    }

    public void UpdateText()
    {
        // Display as a whole number
        valueText.text = "- " + Mathf.RoundToInt(value * textValueMultiplier).ToString();
    }
}
