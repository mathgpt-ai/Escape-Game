using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour, IInteractable
{
    public Scene3Trigger scene3;
    public string lightsTag = "AlarmLight";
    public Material material;
    public bool alarm = true;
    private Canvas canvas;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (alarm)
        {
            float intensity = Mathf.Sin(Time.time * 4f) * 0.5f + 0.5f;
            Color baseColor = Color.red * intensity;
            material.SetColor("_EmissionColor", baseColor);
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            material.SetColor("_EmissionColor", Color.white);
        }
    }

    public void Interact()
    {
        if (alarm)
        {
            alarm = false;

            if (scene3 != null && scene3.IsActive)
            {
                // pour ytrouver tt les lumières/ neon
                GameObject[] lights = GameObject.FindGameObjectsWithTag(lightsTag);
                Debug.Log(lights.Length);
                foreach (GameObject light in lights)
                {
                    // désactive le script
                    LEDNode ledNode = light.GetComponent<LEDNode>();
                    if (ledNode != null)
                    {
                        Debug.Log("LEDNode a été désactivé");
                        ledNode.enabled = false;
                    }

                    // change la couleur de la lumière
                    Light lightComponent = light.GetComponent<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.color = Color.white;
                    }

                    // **NEW** : change l'émission du matériel
                    Renderer renderer = light.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material mat = renderer.material;
                        if (mat != null && mat.HasProperty("_EmissionColor"))
                        {
                            mat.SetColor("_EmissionColor", Color.white);
                        }
                    }
                }
            }
        }
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}