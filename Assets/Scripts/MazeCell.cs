using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject frontWall;

    [SerializeField] private GameObject backWall;

    [SerializeField] private GameObject leftWall;

    [SerializeField] private GameObject rightWall;

    [SerializeField] private GameObject UnvisitedCell;

    public bool IsVisited { get; private set; }


    public void Visited()
    {
        IsVisited= true;

        UnvisitedCell.SetActive(false);

    }

    public void ClearWall(char wallName)
    {
        switch (wallName)
        {
            case 'r':
                rightWall.SetActive(false);
                break;
            case 'l':
                leftWall.SetActive(false);
                break;
            case 'f':
                frontWall.SetActive(false);
                break;
            case 'b':
                backWall.SetActive(false);
                break;
        }
    }




}
