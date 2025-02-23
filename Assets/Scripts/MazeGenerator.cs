using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth; // Largeur du labyrinthe

    [SerializeField]
    private int _mazeDepth; // Profondeur du labyrinthe

    private MazeCell[,] _mazeGrid;
    [SerializeField]  float CellSize = 5f; // Facteur d'agrandissement

    [SerializeField]private GameObject dragon1;
    [SerializeField] private GameObject dragon2;
    [SerializeField] private GameObject dragon3;
    [SerializeField] private GameObject door;


    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Création de la grille avec toutes les cellules
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * CellSize, 0, z * CellSize), // Multiplie les positions par 5
                    Quaternion.identity
                );
            }
        }

        GenerateMazeWithRandomizedDFS();
        CreateEntryAndExit(); // Ajoute une entrée et une sortie au labyrinthe
        SpawnDragons();
        
    }

    // 🔹 Génération du labyrinthe avec DFS aléatoire amélioré
    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0]; // Cellule de départ
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // 20% de chance de choisir une cellule aléatoire dans la pile
            if (UnityEngine.Random.value < 0.2f && stack.Count > 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, stack.Count - 1);
                currentCell = stack.ElementAt(randomIndex);
                stack = new Stack<MazeCell>(stack.Where(c => c != currentCell)); // Supprime de la pile
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
                stack.Pop(); // Retour en arrière
            }
        }
    }

    // 🔹 Sélection d'une cellule non visitée aléatoirement
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        unvisitedCells = unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).ToList(); // Mélange des cellules

        return unvisitedCells.FirstOrDefault();
    }

    // 🔹 Récupère les cellules adjacentes non visitées
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x / CellSize); // Ajuste les coordonnées
        int z = (int)(currentCell.transform.position.z / CellSize);

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited) yield return cellToRight;
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited) yield return cellToLeft;
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited) yield return cellToFront;
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited) yield return cellToBack;
        }
    }

    // 🔹 Suppression des murs entre deux cellules adjacentes
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        Vector3 prevPos = previousCell.transform.position;
        Vector3 currPos = currentCell.transform.position;

        if (prevPos.x < currPos.x) // Droite
        {
            if (currPos.x < _mazeWidth * CellSize) previousCell.ClearRightWall();
            if (prevPos.x >= 0) currentCell.ClearLeftWall();
        }
        else if (prevPos.x > currPos.x) // Gauche
        {
            if (currPos.x >= 0) previousCell.ClearLeftWall();
            if (prevPos.x < _mazeWidth * CellSize) currentCell.ClearRightWall();
        }
        else if (prevPos.z < currPos.z) // Avant
        {
            if (currPos.z < _mazeDepth * CellSize) previousCell.ClearFrontWall();
            if (prevPos.z >= 0) currentCell.ClearBackWall();
        }
        else if (prevPos.z > currPos.z) // Arrière
        {
            if (currPos.z >= 0) previousCell.ClearBackWall();
            if (prevPos.z < _mazeDepth * CellSize) currentCell.ClearFrontWall();
        }
    }

    // 🔹 Création d'une entrée et d'une sortie
    private void CreateEntryAndExit()
    {
        // Entrée : Cellule en bas à gauche
        _mazeGrid[0, 0].ClearBackWall(); // Ouvre vers l'extérieur

        // Sortie : Cellule en haut à droite
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall(); // Ouvre vers l'extérieur
        Vector3 end = new Vector3(_mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.x, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.y, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.z + 2);
        Instantiate(door,end,Quaternion.identity);

       

        


    }

    private void SpawnDragons()
    {
        // On va placer 3 dragons, en utilisant dragon1, dragon2 et dragon3
        GameObject[] dragons = { dragon1, dragon2, dragon3 };

        for (int i = 0; i < 3; i++)
        {
            MazeCell randomCell = GenerateRandomCell();
            InstantiateDragon(randomCell, dragons[i]);
        }
    }

    private void InstantiateDragon(MazeCell cell, GameObject dragonPrefab)
    {
        List<string> availableWalls = new List<string>();

        int cellX = (int)(cell.transform.position.x / CellSize);
        int cellY = (int)(cell.transform.position.z / CellSize);

        // On check les murs actifs, mais faut pas prendre ceux qui sont vers l'extérieur
        if (cell.IsBackWallActive && cellY > 0) // Pas en bas de la sortie
        {
            availableWalls.Add("Back");
        }
        if (cell.IsFrontWallActive && cellY < _mazeDepth - 1) // Pas en haut de la sortie
        {
            availableWalls.Add("Front");
        }
        if (cell.IsLeftWallActive && cellX > 0) // Pas à gauche de la sortie
        {
            availableWalls.Add("Left");
        }
        if (cell.IsRightWallActive && cellX < _mazeWidth - 1) // Pas à droite de la sortie
        {
            availableWalls.Add("Right");
        }

        // Si on a au moins un mur disponible, on place un dragon
        if (availableWalls.Count > 0)
        {
            string chosenWall = availableWalls[UnityEngine.Random.Range(0, availableWalls.Count)];

            // On place le dragon sur le mur sélectionné
            switch (chosenWall)
            {
                case "Back": // Face vers l'avant (Z+)
                    Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, -1.5f), Quaternion.Euler(90, 0, 0));
                    break;
                case "Front": // Face vers l'arrière (Z-)
                    Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, 1.5f), Quaternion.Euler(90, 180, 0));
                    break;
                case "Left": // Face vers la droite (X+)
                    Instantiate(dragonPrefab, cell.transform.position + new Vector3(-1.5f, 1, 0), Quaternion.Euler(90, 90, 0));
                    break;
                case "Right": // Face vers la gauche (X-)
                    Instantiate(dragonPrefab, cell.transform.position + new Vector3(1.5f, 1, 0), Quaternion.Euler(90, -90, 0));
                    break;
            }


        }
    }

    // Permet de choisir une cellule aléatoire, incluant les bords mais pas à l'extérieur du labyrinthe
    private MazeCell GenerateRandomCell()
    {
        int randomX = UnityEngine.Random.Range(0, _mazeWidth);
        int randomZ = UnityEngine.Random.Range(0, _mazeDepth);
        return _mazeGrid[randomX, randomZ];
    }

}
