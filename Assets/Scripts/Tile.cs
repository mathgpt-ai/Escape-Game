using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y; // Coordonnées de la tuile
    private MeshRenderer meshRenderer;
    private Collider tileCollider;
    public float speed = 1.5f;
    private float t = 0.0f;
    private bool isDissolving = false;
    private GridManager gridManager; // Référence au GridManager
    private float timer =0.0f;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        tileCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isDissolving)
        {
            
            t += Time.deltaTime * speed;
            float dissolveAmount = Mathf.Sin(t);
            Material[] mats = meshRenderer.materials;
            mats[0].SetFloat("_Cutoff", dissolveAmount);
            meshRenderer.materials = mats;
        }
    }

    public void Dissolve()
    {
        if (!isDissolving)
        {
            isDissolving = true;
            tileCollider.enabled = false;
        }
    }

    public void SetCoordinates(int x, int y, GridManager manager)
    {
        this.x = x;
        this.y = y;
        this.gridManager = manager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!gridManager.IsTileOnPath(x, y))
            {
                Dissolve();
            }
        }
    }
}

