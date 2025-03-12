using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // Le prefab de la tuile
    public int width = 10;
    public int height = 5;
    public float tileSize = 1.0f;
    private List<(int, int)>[] paths;
    private List<(int, int)> currentPath;

    void Start()
    { 
        GeneratePaths();
        GenerateGrid();
    }
    private void GeneratePaths()
    {
        paths = new List<(int, int)>[3];
        paths[0] = new List<(int, int)> { (1, 1), (1, 2), (1, 3), (1, 4), (1, 5) };
        paths[1] = new List<(int, int)> { (2, 1), (2, 2), (2, 3), (2, 4), (2, 5) };
        paths[2] = new List<(int, int)> { (3, 1), (3, 2), (3, 3), (3, 4), (3, 5) };

        int index = Random.Range(0, paths.Length);

        Debug.Log("Index choisi: " + index);

        currentPath = new List<(int, int)>(paths[index]);

        Debug.Log("Chemin sélectionné: " + string.Join(" -> ", currentPath));
    }


    private void GenerateGrid()
    {
        Vector3 startPosition = transform.position;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = startPosition + new Vector3(x * tileSize, 0, y * tileSize);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                tile.name = $"Tile ({x}, {y})";

                // Assigner les coordonnées à la tuile
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.SetCoordinates(x, y);

                    if (currentPath != null && currentPath.Contains((x, y)))
                    {
                        Debug.Log("Contient");
                        tile.GetComponent<Renderer>().material.color = Color.green; // Change la couleur en vert
                    }
                }
            }
        }
    }
    private void Path()
    {
        //if(PlayerInteract.rigidBody)

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
        {
            Debug.Log("Tile script not found on object.");
            return;
        }

        (int, int) playerPosition = (tile.x, tile.y);
        Debug.Log($"Player stepped on tile: ({playerPosition.Item1}, {playerPosition.Item2})");

        if (!currentPath.Contains(playerPosition))
        {
            Debug.Log("Tuile incorrecte ! Le joueur tombe dans l’espace...");
            tile.Dissolve();
        }
        else
        {
            Debug.Log("Le joueur est sur une tuile correcte !");
        }
    }
}
