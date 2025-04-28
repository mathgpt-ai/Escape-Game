using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetOxygen(float o2)
    {
        slider.value = o2;
    }

    public void SetMaxOxygen(float o2)
    {
        slider.maxValue = o2;
        slider.value = o2;
    }

    public float GetCurrentOxygen()
    {
        return slider.value;
    }
}
