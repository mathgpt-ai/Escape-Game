using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth = 10;
    [SerializeField] private int _mazeDepth = 10;
    private MazeCell[,] _mazeGrid;
    [SerializeField] private float CellSize = 5f;

    [SerializeField] private GameObject dragon1;
    [SerializeField] private GameObject dragon2;
    [SerializeField] private GameObject dragon3;
    [SerializeField] private GameObject door;

    private List<MazeCell> usedCells = new List<MazeCell>();
    private Dictionary<MazeCell, List<string>> usedWalls = new Dictionary<MazeCell, List<string>>();

    // Store existing objects to clean up during regeneration
    private List<GameObject> generatedObjects = new List<GameObject>();

    void Start()
    {
        GenerateMaze();
    }

    // Public method to regenerate the maze
    public void RegenerateMaze()
    {
        // Clean up existing maze elements
        CleanupExistingMaze();

        // Generate a new maze
        GenerateMaze();
    }

    // Clean up any existing maze elements
    private void CleanupExistingMaze()
    {
        // Clear our collections first
        usedCells.Clear();
        usedWalls.Clear();

        // Destroy all tracked generated objects
        foreach (GameObject obj in generatedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        generatedObjects.Clear();

        // Clean up grid cells
        if (_mazeGrid != null)
        {
            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    if (_mazeGrid[x, z] != null)
                    {
                        Destroy(_mazeGrid[x, z].gameObject);
                    }
                }
            }
        }

        // Find and destroy all dragons if they weren't tracked
        GameObject[] dragons = GameObject.FindGameObjectsWithTag("Dragon");
        foreach (GameObject dragon in dragons)
        {
            if (dragon != null)
            {
                Destroy(dragon);
            }
        }

        // Find and destroy all doors if they weren't tracked
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject d in doors)
        {
            if (d != null)
            {
                Destroy(d);
            }
        }
    }

    // Main maze generation method
    private void GenerateMaze()
    {
        // Initialize the maze grid
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Create all cells
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                MazeCell newCell = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * CellSize, 0, z * CellSize),
                    Quaternion.identity,
                    transform
                );

                _mazeGrid[x, z] = newCell;
                generatedObjects.Add(newCell.gameObject);
            }
        }

        // Generate the maze paths
        GenerateMazeWithRandomizedDFS();

        // Create entry and exit
        CreateEntryAndExit();

        // Spawn dragons
        SpawnDragons();

        // Disable minimap initially
        StartCoroutine(DisableMiniMapAfterDelay());
    }

    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0];
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // Occasionally, pick a random cell from the stack rather than the most recent one
            if (UnityEngine.Random.value < 0.2f && stack.Count > 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, stack.Count - 1);
                currentCell = stack.ElementAt(randomIndex);
                stack = new Stack<MazeCell>(stack.Where(c => c != currentCell));
            }
            else
            {
                currentCell = stack.Peek();
            }

            // Find an unvisited neighbor
            MazeCell nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                // Create a path between cells
                ClearWalls(currentCell, nextCell);
                nextCell.Visit();
                stack.Push(nextCell);
            }
            else
            {
                // No unvisited neighbors, backtrack
                stack.Pop();
            }
        }
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        // Get all unvisited neighbors in random order
        var unvisitedCells = GetUnvisitedCells(currentCell).OrderBy(_ => UnityEngine.Random.Range(1, 10)).ToList();
        return unvisitedCells.FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        // Calculate grid coordinates from world position
        int x = Mathf.RoundToInt(currentCell.transform.position.x / CellSize);
        int z = Mathf.RoundToInt(currentCell.transform.position.z / CellSize);

        // Check all four directions for unvisited cells
        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        // Calculate the direction from previous to current cell
        Vector3 direction = currentCell.transform.position - previousCell.transform.position;

        if (direction.x > 0) // Current is to the right of previous
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (direction.x < 0) // Current is to the left of previous
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (direction.z > 0) // Current is in front of previous
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (direction.z < 0) // Current is behind previous
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private void CreateEntryAndExit()
    {
        // Clear the entrance (back wall of first cell)
        _mazeGrid[0, 0].ClearBackWall();

        // Clear the exit (front wall of last cell)
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall();

        // Create the door at the exit
        Vector3 end = new Vector3(_mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.x,
                                 _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.y,
                                 _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.z + 2);

        // Instantiate door at exit
        GameObject exitDoor = Instantiate(door, end, Quaternion.identity);
        exitDoor.tag = "Door"; // Tag for easy finding later
        generatedObjects.Add(exitDoor);
    }

    private void SpawnDragons()
    {
        GameObject[] dragonPrefabs = { dragon1, dragon2, dragon3 };
        int attempts = 0;

        for (int i = 0; i < dragonPrefabs.Length; i++)
        {
            MazeCell randomCell;
            List<string> availableWalls;

            // Find a suitable cell that hasn't been used yet and has available walls
            do
            {
                randomCell = GenerateRandomCell();
                availableWalls = GetAvailableWalls(randomCell);
                attempts++;

                if (attempts > 1000)
                {
                    Debug.LogWarning("Failed to place all dragons due to wall limitations.");
                    return;
                }

            } while (usedCells.Contains(randomCell) || availableWalls.Count == 0);

            // Mark this cell as used for dragon placement
            usedCells.Add(randomCell);

            // Place the dragon
            GameObject dragonInstance = InstantiateDragon(randomCell, dragonPrefabs[i]);
            if (dragonInstance != null)
            {
                generatedObjects.Add(dragonInstance);
            }
        }

        if (usedCells.Count < 3)
        {
            Debug.LogWarning($"Only {usedCells.Count} dragons were placed instead of 3.");
        }
    }

    private List<string> GetAvailableWalls(MazeCell cell)
    {
        List<string> availableWalls = new List<string>();

        // Calculate grid position
        int cellX = Mathf.RoundToInt(cell.transform.position.x / CellSize);
        int cellZ = Mathf.RoundToInt(cell.transform.position.z / CellSize);

        // Check which walls are active and within maze bounds
        if (cell.IsBackWallActive && cellZ > 0) availableWalls.Add("Back");
        if (cell.IsFrontWallActive && cellZ < _mazeDepth - 1) availableWalls.Add("Front");
        if (cell.IsLeftWallActive && cellX > 0) availableWalls.Add("Left");
        if (cell.IsRightWallActive && cellX < _mazeWidth - 1) availableWalls.Add("Right");

        return availableWalls;
    }

    private GameObject InstantiateDragon(MazeCell cell, GameObject dragonPrefab)
    {
        List<string> freeWalls = GetAvailableWalls(cell);
        if (freeWalls.Count == 0) return null;

        // Choose a random available wall
        string chosenWall = freeWalls[UnityEngine.Random.Range(0, freeWalls.Count)];

        // Track which walls have been used for this cell
        usedWalls[cell] = usedWalls.ContainsKey(cell) ? usedWalls[cell] : new List<string>();
        usedWalls[cell].Add(chosenWall);

        // Position and rotation depends on the chosen wall
        GameObject dragonInstance = null;
        switch (chosenWall)
        {
            case "Back":
                dragonInstance = Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, -1.5f), Quaternion.Euler(90, 0, 0));
                break;
            case "Front":
                dragonInstance = Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, 1.5f), Quaternion.Euler(90, 180, 0));
                break;
            case "Left":
                dragonInstance = Instantiate(dragonPrefab, cell.transform.position + new Vector3(-1.5f, 1, 0), Quaternion.Euler(90, 90, 0));
                break;
            case "Right":
                dragonInstance = Instantiate(dragonPrefab, cell.transform.position + new Vector3(1.5f, 1, 0), Quaternion.Euler(90, -90, 0));
                break;
        }

        // Tag the dragon for easy finding later
        if (dragonInstance != null)
        {
            dragonInstance.tag = "Dragon";
        }

        return dragonInstance;
    }

    private MazeCell GenerateRandomCell()
    {
        return _mazeGrid[UnityEngine.Random.Range(0, _mazeWidth), UnityEngine.Random.Range(0, _mazeDepth)];
    }

    private IEnumerator DisableMiniMapAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        if (MiniMap.Instance != null)
        {
            MiniMap.Instance.DisableMiniMap();
        }
    }
}