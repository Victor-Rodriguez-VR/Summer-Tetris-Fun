using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{   //classic tetris games always have width set to 10, and height set to 20.
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool inGrid(Vector2 pos) {
        //confirms tetrimino is within bounds of the x-plane axis, and the y plane axis.
    return ((int)pos.x >=0 && (int)pos.x< gridWidth && (int)pos.y >= 0)    ;
    }

    //Helper method, helps rounds :)
    public Vector2 Round(Vector2 pos) {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }
}
