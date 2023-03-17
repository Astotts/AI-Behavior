using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public int g, h;
    public int f { get { return g + h; } }
    
    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    //Enables the overlay tile
    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    //Disables the overlay tile
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
