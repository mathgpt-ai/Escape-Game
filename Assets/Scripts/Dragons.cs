using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragons : MonoBehaviour,IInteractable
{
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    public void Interact()
    {
        AudioSource.PlayClipAtPoint(clip,transform.position);
        gameObject.SetActive(false);
    }
}
