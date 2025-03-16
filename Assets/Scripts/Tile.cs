using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y; // Coordonn�es de la tuile
    private MeshRenderer meshRenderer;
    private Collider tileCollider;
    public float speed = 1.5f;
    private float t = 0.0f;
    private bool isDissolving = false;
    private GridManager gridManager; // R�f�rence au GridManager

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

            if (t >= Mathf.PI / 2)
            {
                Destroy(gameObject);
            }
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
        this.gridManager = manager; // Assigner le GridManager
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // V�rifie si c'est bien le joueur
        {
            Debug.Log($"Joueur sur tuile ({x}, {y})");

            if (!gridManager.IsTileOnPath(x, y)) // V�rifie si la tuile fait partie du chemin
            {
                Debug.Log("Tuile incorrecte ! Le joueur tombe dans l�espace...");
                Dissolve();
            }
            else
            {
                Debug.Log("Le joueur est sur une tuile correcte !");
            }
        }
    }
}

