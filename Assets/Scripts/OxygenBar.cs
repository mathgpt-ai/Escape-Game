using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetOxygen(int o2)
    {
        slider.value = o2;
    }

    public void SetMaxOxygen(int o2)
    {
        slider.maxValue = o2;
        slider.value = o2;
    }
}
