using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float MaxValue)
    {
        slider.maxValue = MaxValue;
    }
    public void SetValue(float Value)
    {
        slider.value = Value;
    }
}