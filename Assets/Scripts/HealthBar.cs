using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }

    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetHealth(float h) => slider.value = h;
    public float GetCurrentHealth() => slider.value;
    public float GetMaxHealth() => slider.maxValue;
}
