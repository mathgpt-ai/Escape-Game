using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip sprintSound;
    public FirstPersonController controller;
    public zerograv zerograv;
    KeyCode[] keysToCheck =
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
    };

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Update()
    {
        bool isMoving = false;

        if (!controller.IsGrounded || zerograv.isActiveAndEnabled)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            return;
        }

        foreach (var key in keysToCheck)
        {
            if (Input.GetKey(key))
            {
                isMoving = true;
                break;
            }
        }

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            AudioClip desiredClip = isSprinting ? sprintSound : walkSound;

            if (audioSource.clip != desiredClip)
            {
                audioSource.clip = desiredClip;
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
