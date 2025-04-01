using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    GameObject Player;
    [SerializeField] float CamHeight;
    private float fixedXRotation;
    private float fixedZRotation;
    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
        fixedXRotation = transform.rotation.eulerAngles.x;
        fixedZRotation = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (Player !=null)
        {
            this.gameObject.transform.position = new Vector3(Player.transform.position.x, CamHeight, Player.transform.position.z);
            transform.rotation = Quaternion.Euler(fixedXRotation, Player.transform.eulerAngles.y, fixedZRotation);
        }
    }

    IEnumerator FindPlayerCoroutine()
    {
        yield return new WaitForSeconds(5f);

        Player = GameObject.FindGameObjectWithTag("Player");
        
    }
}
