using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEffect : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    public void StartEffect()
    {
        particleSystem.Play();
    }

    public void StopEffect()
    {
        particleSystem.Stop();
    }
}
