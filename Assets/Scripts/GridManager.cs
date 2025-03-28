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
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public Transform canvasSpawnPoint;
    int index;

    void Start()
    {
        GeneratePaths();
        GenerateGrid();
        spawnCanvas();
    }

    private void GeneratePaths()
    {
        List<(int, int)>[] paths = new List<(int, int)>[3];
        paths[0] = new List<(int, int)> { (4, 0), (4, 1), (4, 2), (3, 2), (2, 2), (2, 1), (1, 1), (0, 1), (1, 2), (0, 2), (0, 3), (0, 4), (0, 5), (1, 5), (2, 5), (2, 6), (2, 7), (3, 7), (4, 7), (4, 8), (4, 9), (3, 9), (2, 9), (2, 10) };
        paths[1] = new List<(int, int)> { (2, 0), (2, 1), (3, 1), (4, 1), (4, 2), (4, 3), (3, 3), (3, 4), (3, 5), (2, 5), (1, 5), (0, 5), (0, 6), (0, 7), (0, 8), (0, 9), (1, 9), (1, 10) };
        paths[2] = new List<(int, int)> { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (1, 5), (2, 5), (2, 4), (2, 3), (2, 2), (3, 2), (4, 2), (4, 3), (4, 4), (4, 5), (4, 6), (4, 7), (4, 8), (4, 9), (3, 9), (2, 9), (1, 9), (1, 10) };

        index = Random.Range(0, paths.Length);
        currentPath = new List<(int, int)>(paths[index]);
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
    public void spawnCanvas()
    {
        Debug.Log("Index choisi : " + index);

        if (index == 0)
        {
            Debug.Log("Canvas 1 spawn !");
            Instantiate(canvas1, canvasSpawnPoint);
        }
        if (index == 1)
        {
            Debug.Log("Canvas 2 spawn !");
            Instantiate(canvas2, canvasSpawnPoint);
        }
        if (index == 2)
        {
            Debug.Log("Canvas 3 spawn !");
            Instantiate(canvas3, canvasSpawnPoint);
        }
    }
    public bool IsTileOnPath(int x, int y)
    {
        return currentPath.Contains((x, y));
    }
}
