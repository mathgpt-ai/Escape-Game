using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // j'ai utilisé un asset pour faire le Disolving mais je l'ai modifié à ma façon.

    public int x, y; // Coordonnées de la tuile
    private MeshRenderer meshRenderer;
    private Collider tileCollider;
    public float speed = 1.5f;
    private float t = 0.0f;
    private bool isDissolving = false;
    private GridManager gridManager; // Référence au GridManager
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
            float dissolveAmount = Mathf.Clamp01(t); // Assure que ça reste entre 0 et 1

            Material[] mats = meshRenderer.materials;
            mats[0].SetFloat("_Cutoff", dissolveAmount);
            meshRenderer.materials = mats;

            // Désactiver l'objet une fois la disparition complète
            if (t >= 1.0f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Dissolve()
    {
        if (!isDissolving)
        {
            isDissolving = true;
            tileCollider.enabled = false;
            Invoke("Respawn", 3.0f);
        }
    }
    private void Respawn()
    {
        gameObject.SetActive(true);
        isDissolving = false;
        t = 0.0f; // Reset la progression
        tileCollider.enabled = true;

        Material[] mats = meshRenderer.materials;
        mats[0].SetFloat("_Cutoff", 0);
        meshRenderer.materials = mats;
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

