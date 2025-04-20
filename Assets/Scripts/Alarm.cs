using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public Material material;
    public bool alarm = true;
    Color otherColor = new Color(1,1,1);
    private void Update()
    {
        if(alarm)
        {
            float intensity = Mathf.Sin(Time.time * 4f) * 0.5f + 0.5f;
            Color baseColor = Color.red * intensity;

            material.SetColor("_EmissionColor", baseColor);
        }
        else
        {
            Color whiteColor = Color.white * 1f; // 1f = emission intensity
            material.SetColor("_EmissionColor", whiteColor);
        }
    }
    
    

}
