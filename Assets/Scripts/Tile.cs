using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y; // Coordonn�es de la tuile

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
