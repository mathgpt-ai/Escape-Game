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
        GenerateGrid();
        GeneratePaths();
        SelectRandomPath();
        
    }
    private void GeneratePaths()
    {
        paths = new List<(int, int)>[3];
        paths[0] = new List<(int, int)> { (0, 2), (1, 2), (2, 2), (3, 3) };
        paths[1] = new List<(int, int)> { (0, 1), (1, 1), (2, 2), (3, 2) };
        paths[2] = new List<(int, int)> { (0, 3), (1, 3), (2, 3), (3, 4) };
    }
    private void SelectRandomPath()
    {
        int index = Random.Range(0, paths.Length);

        Debug.Log("Index choisi: " + index);

        currentPath = new List<(int, int)>(paths[index]);

        Debug.Log("Chemin sélectionné: " + string.Join(" -> ", currentPath));
    }

    private void GenerateGrid()
    {
        Vector3 startPosition = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = startPosition + new Vector3(x * tileSize, 0, y * tileSize);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                tile.name = $"Tile ({x}, {y})";

                // Assigner les coordonnées à la tuile
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.SetCoordinates(x, y);

                    // Vérifier si cette tuile fait partie du chemin
                    if (currentPath != null && currentPath.Contains((x, y)))
                    {
                        tile.GetComponent<Renderer>().material.color = Color.green; // Change la couleur en vert
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Tile tile = other.GetComponent<Tile>();

        (int, int) playerPosition = (tile.x, tile.y);

        if (currentPath.Contains(playerPosition))
        {
            Debug.Log("Le joueur est sur une tuile correcte !");
        }
        else
        {
            Debug.Log("Tuile incorrecte ! Le joueur tombe dans l’espace...");
        }
    }
}
