using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport2 : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        print("TMPNIGGER");
        collision.gameObject.transform.position = GameObject.Find("SpawnP").transform.position;

    }

}

