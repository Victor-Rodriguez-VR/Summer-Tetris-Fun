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
        spawnNextTetrimino();
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

    //Instantiates a prefab.
    public void spawnNextTetrimino() {
        GameObject nextTetrimino =  (GameObject)Instantiate(Resources.Load(getRandomTetrimino(), typeof(GameObject)), new Vector2(5.0f,20.0f),  Quaternion.identity);
    }

    string getRandomTetrimino() {
        int randomTetrimino = Random.Range(1, 8); // 1 to 7
        string randomTetriminoName = "Prefabs/Tetris_L";
        switch (randomTetrimino) {
            case 1:
                randomTetriminoName = "Prefabs/Tetris_L";
                break;
            case 2:
                randomTetriminoName = "Prefabs/Tetris_Long";
                break;
            case 3:
                randomTetriminoName = "Prefabs/Tetris_J";
                break;
            case 4:
                randomTetriminoName = "Prefabs/Tetris_Square";
                break;
            case 5:
                randomTetriminoName = "Prefabs/Tetris_S";
                break;
            case 6:
                randomTetriminoName = "Prefabs/Tetris_T";
                break;
            case 7:
                randomTetriminoName = "Prefabs/Tetris_Z";
                break;
        }
        return randomTetriminoName;
    }
}
