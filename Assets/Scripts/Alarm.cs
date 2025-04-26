using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour, IInteractable
{
    public Material material;
    public bool alarm = true;
    private Canvas canvas;

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
            Color whiteColor = Color.white * 1f;
            material.SetColor("_EmissionColor", Color.white);
        }
    }
    
    public void Interact()
    {
        if(alarm)
        {
            alarm = false;
        }
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

}
