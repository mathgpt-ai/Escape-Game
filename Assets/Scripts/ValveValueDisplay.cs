using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValveValueDisplay : MonoBehaviour
{
    public enum GasType { Oxygene, Azote, DioxydeDeCarbone }

    [SerializeField] private WheelInteract wheel;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GasType gazType;

    void Start()
    {
        if (wheel == null) return;

        switch (gazType)
        {
            case GasType.Azote:
                wheel.SetStartValue(53f);
                break;
            case GasType.Oxygene:
                wheel.SetStartValue(9f);
                break;
            case GasType.DioxydeDeCarbone:
                wheel.SetStartValue(6f);
                break;
        }
    }
    void Update()
    {
        if (wheel != null && valueText != null)
        {
            float rotation = wheel.GetCurrentRotation(); // 0 à 360
            float value = rotation / 360f * 100f;

            // Affichage avec ou sans décimales selon le gaz
            string formattedValue = gazType == GasType.DioxydeDeCarbone ? $"{value:F1}%" : $"{value:F0}%";

            valueText.text = formattedValue;
            valueText.color = GetColorForValue(gazType, value);
        }
    }

    private Color GetColorForValue(GasType type, float value)
    {
        switch (type)
        {
            case GasType.Azote:
                if (Mathf.Abs(value - 78f) < 0.5f) return Color.green;
                if (value >= 70f && value <= 90f) return new Color(1f, 0.5f, 0f);// orange
                return Color.red;

            case GasType.Oxygene:
                if (Mathf.Abs(value - 21f) < 0.5f) return Color.green;
                if (value >= 10f && value <= 30f) return new Color(1f, 0.5f, 0f); // orange
                return Color.red;

            case GasType.DioxydeDeCarbone:
                if (value >= 0.1f && value <= 0.5f) return Color.green;
                if ((value >= -4.9f && value < 0.1f) || (value > 0.5f && value <= 5.5f)) return new Color(1f, 0.5f, 0f);// orange
                return Color.red;

            default:
                return Color.white;
        }
    }

}

