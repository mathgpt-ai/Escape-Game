using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip countdown;
    [SerializeField] private AudioClip poweringUp;
    [SerializeField] private string lightsTag = "AlarmLight";
    [SerializeField] private Material material;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestObject questObject;
    public Scene3Trigger scene3;

    Canvas canvas;

    private AudioSource audioSource;
    private float timer = 0f;
    private bool alarm = true;

    private bool hasPlayedCountdown = false;
    private bool hasPlayedPoweringUp = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (alarm)
            HandleAlarmEffects();
        else
            HandleAlarmOffEffects();
    }

    private void HandleAlarmEffects()
    {
        float intensity = Mathf.Sin(Time.time * 4f) * 0.5f + 0.5f;
        Color baseColor = Color.red * intensity;
        material.SetColor("_EmissionColor", baseColor);

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Réinitialisation des états si l'alarme se rallume
        hasPlayedCountdown = false;
        hasPlayedPoweringUp = false;
        timer = 0f;
    }

    private void HandleAlarmOffEffects()
    {
        if (!hasPlayedCountdown)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(countdown);
            hasPlayedCountdown = true;
        }

        timer += Time.deltaTime;

        if (timer > 10.2f && !hasPlayedPoweringUp)
        {
            audioSource.PlayOneShot(poweringUp);
            hasPlayedPoweringUp = true;
        }

        material.SetColor("_EmissionColor", Color.white);
    }

    public void Interact()
    {
        if (alarm)
        {
            alarm = false;
            timer = 0f;

            if (scene3 != null && scene3.IsActive)
            {
                GameObject[] lights = GameObject.FindGameObjectsWithTag(lightsTag);
                Debug.Log(lights.Length);

                foreach (GameObject light in lights)
                {
                    // Désactiver script LEDNode
                    LEDNode ledNode = light.GetComponent<LEDNode>();
                    if (ledNode != null)
                    {
                        Debug.Log("LEDNode a été désactivé");
                        ledNode.enabled = false;
                    }

                    // Changer la couleur de la lumière
                    Light lightComponent = light.GetComponent<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.color = Color.white;
                    }

                    // Changer l'émission du matériel
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
            if (questManager != null && questObject != null)
            {
                questManager.ForcerComplétion(questObject);
            }
        }
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}