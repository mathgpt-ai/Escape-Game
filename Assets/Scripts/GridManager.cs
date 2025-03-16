using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 10;
    public int height = 5;
    public float tileSize = 1.0f;
    private List<(int, int)> currentPath;

    void Start()
    {
        GeneratePaths();
        GenerateGrid();
    }

    private void GeneratePaths()
    {
        List<(int, int)>[] paths = new List<(int, int)>[3];
        paths[0] = new List<(int, int)> { (1, 1), (1, 2), (1, 3), (1, 4), (1, 5) };
        paths[1] = new List<(int, int)> { (2, 1), (2, 2), (2, 3), (2, 4), (2, 5) };
        paths[2] = new List<(int, int)> { (3, 1), (3, 2), (3, 3), (3, 4), (3, 5) };

        int index = Random.Range(0, paths.Length);
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

                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.SetCoordinates(x, y, this); // Passe GridManager à Tile

                    if (IsTileOnPath(x, y))
                    {
                        tile.GetComponent<Renderer>().material.color = Color.green;
                    }
                }
            }
        }
    }

    public bool IsTileOnPath(int x, int y)
    {
        return currentPath.Contains((x, y));
    }
}
