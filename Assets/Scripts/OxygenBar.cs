using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public static OxygenBar Instance { get; private set; }

    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetOxygen(float o2) => slider.value = o2;
    public float GetCurrentOxygen() => slider.value;
    public float GetMaxOxygen() => slider.maxValue;
}
