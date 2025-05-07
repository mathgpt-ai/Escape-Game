using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject player;

    private void Start()
    {
        int index = PlayerPrefs.GetInt("SpawnPointIndex", 0);
        player.transform.position = spawnPoints[Mathf.Clamp(index, 0, spawnPoints.Length - 1)].position;

        if (PlayerPrefs.GetInt("DisableOxygenSystem", 0) == 1)
        {
            GameObject oxygenSystem = GameObject.Find("OxygenSystem");
            if (oxygenSystem != null)
                oxygenSystem.SetActive(false);
        }
    }
}
