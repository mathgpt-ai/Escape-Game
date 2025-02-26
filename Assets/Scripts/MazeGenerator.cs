using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth; // Largeur du labyrinthe
    [SerializeField] private int _mazeDepth; // Profondeur du labyrinthe
    private MazeCell[,] _mazeGrid;
    [SerializeField] private float CellSize = 5f; // Facteur d'agrandissement

    [SerializeField] private GameObject dragon1;
    [SerializeField] private GameObject dragon2;
    [SerializeField] private GameObject dragon3;
    [SerializeField] private GameObject door;

    private List<MazeCell> usedCells = new List<MazeCell>(); // Pour garder en mémoire les cellules utilisées
    private Dictionary<MazeCell, List<string>> usedWalls = new Dictionary<MazeCell, List<string>>(); // Pour éviter que deux dragons spawn sur le même mur

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // 🔹 Création de la grille
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * CellSize, 0, z * CellSize),
                    Quaternion.identity
                );
            }
        }

        GenerateMazeWithRandomizedDFS();
        CreateEntryAndExit();
        SpawnDragons();
    }

    // 🔹 Génération du labyrinthe avec DFS
    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0];
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // 20% de chance de piger une cellule aléatoire dans la pile
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

            MazeCell nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                ClearWalls(currentCell, nextCell);
                nextCell.Visit();
                stack.Push(nextCell);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    // 🔹 Récupère une cellule non visitée aléatoirement
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).OrderBy(_ => UnityEngine.Random.Range(1, 10)).ToList();
        return unvisitedCells.FirstOrDefault();
    }

    // 🔹 Vérifie les cellules adjacentes non visitées
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x / CellSize);
        int z = (int)(currentCell.transform.position.z / CellSize);

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    // 🔹 Enlève les murs entre les cellules adjacentes
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        Vector3 prevPos = previousCell.transform.position;
        Vector3 currPos = currentCell.transform.position;

        if (prevPos.x < currPos.x) { previousCell.ClearRightWall(); currentCell.ClearLeftWall(); }
        else if (prevPos.x > currPos.x) { previousCell.ClearLeftWall(); currentCell.ClearRightWall(); }
        else if (prevPos.z < currPos.z) { previousCell.ClearFrontWall(); currentCell.ClearBackWall(); }
        else if (prevPos.z > currPos.z) { previousCell.ClearBackWall(); currentCell.ClearFrontWall(); }
    }

    // 🔹 Ajoute une entrée et une sortie
    private void CreateEntryAndExit()
    {
        _mazeGrid[0, 0].ClearBackWall();
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall();
        Vector3 end = new Vector3(_mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.x,
                                  _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.y,
                                  _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.z + 2);
        Instantiate(door, end, Quaternion.identity);
    }

    // 🔹 Spawn des dragons
    private void SpawnDragons()
    {
        GameObject[] dragons = { dragon1, dragon2, dragon3 };
        int attempts = 0;

        for (int i = 0; i < dragons.Length; i++)
        {
            MazeCell randomCell;
            List<string> availableWalls;

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

            usedCells.Add(randomCell);
            InstantiateDragon(randomCell, dragons[i]);
        }

        if (usedCells.Count < 3)
        {
            Debug.LogError($"Only {usedCells.Count} dragons were placed instead of 3.");
        }
    }

    private List<string> GetAvailableWalls(MazeCell cell)
    {
        List<string> availableWalls = new List<string>();
        int cellX = (int)(cell.transform.position.x / CellSize);
        int cellY = (int)(cell.transform.position.z / CellSize);

        if (cell.IsBackWallActive && cellY > 0) availableWalls.Add("Back");
        if (cell.IsFrontWallActive && cellY < _mazeDepth - 1) availableWalls.Add("Front");
        if (cell.IsLeftWallActive && cellX > 0) availableWalls.Add("Left");
        if (cell.IsRightWallActive && cellX < _mazeWidth - 1) availableWalls.Add("Right");

        return availableWalls;
    }

    // 🔹 Place un dragon sur un mur disponible
    private void InstantiateDragon(MazeCell cell, GameObject dragonPrefab)
    {
        List<string> freeWalls = GetAvailableWalls(cell);
        if (freeWalls.Count == 0) return;

        string chosenWall = freeWalls[UnityEngine.Random.Range(0, freeWalls.Count)];
        usedWalls[cell] = usedWalls.ContainsKey(cell) ? usedWalls[cell] : new List<string>();
        usedWalls[cell].Add(chosenWall);

        switch (chosenWall)
        {
            case "Back": Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, -1.5f), Quaternion.Euler(90, 0, 0)); break;
            case "Front": Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, 1.5f), Quaternion.Euler(90, 180, 0)); break;
            case "Left": Instantiate(dragonPrefab, cell.transform.position + new Vector3(-1.5f, 1, 0), Quaternion.Euler(90, 90, 0)); break;
            case "Right": Instantiate(dragonPrefab, cell.transform.position + new Vector3(1.5f, 1, 0), Quaternion.Euler(90, -90, 0)); break;
        }
    }

    private MazeCell GenerateRandomCell()
    {
        return _mazeGrid[UnityEngine.Random.Range(0, _mazeWidth), UnityEngine.Random.Range(0, _mazeDepth)];
    }
}
