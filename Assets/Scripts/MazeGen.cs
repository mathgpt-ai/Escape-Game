using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;

public class MazeGen : MonoBehaviour
{
    [SerializeField ]private MazeCell MazeCellPref;

     [SerializeField] private int MazeDepth = 10;
   [SerializeField] private int MazeWidth = 20;

    private MazeCell[,] MazeGrid;
    void Start()
    {
        MazeGrid=new MazeCell[MazeWidth,MazeDepth];
        for(int i = 0; i < MazeWidth; i++)
        {
            for(int j = 0; j < MazeDepth; j++)
            {
                MazeGrid[i,j]=Instantiate(MazeCellPref,new Vector3(i,0,j), Quaternion.identity);
            }
        }
         GenerateMaze(null, MazeGrid[0,0]);
    }

    private void GenerateMaze(MazeCell previousCell,MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell Nextcell;

        do
        {
            Nextcell = GetNextUnvisitedCell(currentCell);
            if(Nextcell!=null)
                GenerateMaze(currentCell,Nextcell);

        }while(Nextcell!=null);
    }

    private MazeCell GetNextUnvisitedCell( MazeCell currentCell)
    {
        var unvisitedCells=GetUnvisitedCells(currentCell);

       return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < MazeWidth)
        {
            var cellToRIght = MazeGrid[z + 1, z];

            if (cellToRIght.IsVisited == false)
            {
                yield return cellToRIght;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToleft = MazeGrid[x - 1, z];
            if(cellToleft.IsVisited == false)
               yield return cellToleft;

        }
        if (z + 1 < MazeDepth)
        {
            var cellToFront = MazeGrid[x, z + 1];
            if(cellToFront.IsVisited== false)
               yield return cellToFront;
        }
        if(z-1>= 0)
        {
            var cellToBack = MazeGrid[x, z - 1];
            if (cellToBack.IsVisited == false)
               yield return cellToBack;
        }
        


    }


    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
            return;

        if (previousCell.transform.position.x > currentCell.transform.position.x)//check if curent cell to the left
        {
            previousCell.ClearWall('r');
            currentCell.ClearWall('l');
            return;
        }
            if (previousCell.transform.position.x < currentCell.transform.position.x)//check if curent cell to the left
            {
                previousCell.ClearWall('l');
                currentCell.ClearWall('r');
                return;
            }
            if (previousCell.transform.position.z < currentCell.transform.position.z)//check if curent cell to the left
            {
                previousCell.ClearWall('f');
                currentCell.ClearWall('b');
                return;
            }
            if (previousCell.transform.position.z > currentCell.transform.position.z)//check if curent cell to the left
            {
                previousCell.ClearWall('b');
                currentCell.ClearWall('f');
                return;
            }
        




    }
}
