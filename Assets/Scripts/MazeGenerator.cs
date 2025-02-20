using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // Cr�ation de la grille avec toutes les cellules
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        GenerateMazeWithRandomizedDFS();
        CreateEntryAndExit(); // Ajoute une entr�e et une sortie au labyrinthe
    }

    // ?? G�n�ration du labyrinthe avec DFS al�atoire am�lior�
    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0]; // Cellule de d�part
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // 20% de chance de choisir une cellule al�atoire dans la pile
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
                stack.Pop(); // Retour en arri�re
            }
        }
    }

    // ?? S�lection d'une cellule non visit�e al�atoirement
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        unvisitedCells = unvisitedCells.OrderBy(_ => Random.Range(1, 10)).ToList(); // M�lange des cellules

        return unvisitedCells.FirstOrDefault();
    }

    // ?? R�cup�re les cellules adjacentes non visit�es
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

    // ?? Suppression des murs entre deux cellules adjacentes
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        Vector3 prevPos = previousCell.transform.position;
        Vector3 currPos = currentCell.transform.position;

        if (prevPos.x < currPos.x) // Droite
        {
            if (currPos.x < _mazeWidth) previousCell.ClearRightWall();
            if (prevPos.x >= 0) currentCell.ClearLeftWall();
        }
        else if (prevPos.x > currPos.x) // Gauche
        {
            if (currPos.x >= 0) previousCell.ClearLeftWall();
            if (prevPos.x < _mazeWidth) currentCell.ClearRightWall();
        }
        else if (prevPos.z < currPos.z) // Avant
        {
            if (currPos.z < _mazeDepth) previousCell.ClearFrontWall();
            if (prevPos.z >= 0) currentCell.ClearBackWall();
        }
        else if (prevPos.z > currPos.z) // Arri�re
        {
            if (currPos.z >= 0) previousCell.ClearBackWall();
            if (prevPos.z < _mazeDepth) currentCell.ClearFrontWall();
        }
    }

    // ?? Cr�ation d'une entr�e et d'une sortie
    private void CreateEntryAndExit()
    {
        // Entr�e : Cellule en bas � gauche
        _mazeGrid[0, 0].ClearBackWall(); // Ouvre vers l'ext�rieur

        // Sortie : Cellule en haut � droite
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall(); // Ouvre vers l'ext�rieur
    }
}
