using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Création de la grille du labyrinthe
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMazeWithRandomizedDFS();
        CreateEntryAndExit(); // Ajoute une entrée et une sortie au labyrinthe
    }

    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0]; // Définir la cellule de départ
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // 20% de chance de prendre une cellule aléatoire dans la pile au lieu de la dernière
            if (Random.value < 0.2f && stack.Count > 1)
            {
                int randomIndex = Random.Range(0, stack.Count - 1);
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

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        unvisitedCells = unvisitedCells.OrderBy(_ => Random.Range(1, 10)).ToList(); // Mélanger les cellules

        return unvisitedCells.FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

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

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private void CreateEntryAndExit()
    {
        // Création de l'entrée (première cellule en bas à gauche)
        _mazeGrid[0, 0].ClearLeftWall();

        // Création de la sortie (dernière cellule en haut à droite)
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearRightWall();
    }
}

