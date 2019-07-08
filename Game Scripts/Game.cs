using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game : MonoBehaviour
{   //classic tetris games always have width set to 10, and height set to 20.
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    public int[] scores = new int[] { 40, 100, 300, 1200};
    public Text huddyHUD;
    public int currentPoppedRows = 0;
    public static int currentScore = 0;

    void Start()
    {
        spawnNextTetrimino();
    }
    
    void Update() {
        updateScore();

        updateUI();
    }
    public void updateScore() {
        
        if (currentPoppedRows > 0) {

            currentScore += scores[currentPoppedRows - 1];
            
        }
        currentPoppedRows = 0;


    }

    public void updateUI() {
        huddyHUD.text = currentScore.ToString();
    }
    // Start is called before the first frame update
   

    

    public bool checkIsAboveGrid(Tetrimino tetrimino) {
        for (int x = 0; x < gridWidth; ++x) {
            foreach (Transform mino in tetrimino.transform) {
                Vector2 pos = Round(mino.position);
                if (pos.y > (gridHeight-1)) {
                    return true;
                }
            }
        }
        return false;
    }

    public void gameOver() {
        SceneManager.LoadScene("GameOver");
        

    }


    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public bool isFullAt(int y)
    {
        /// Param y is the current row of we iterate over for a transform.
        for (int x = 0; x < gridWidth; ++x)
        {
            ///While in the row, if any index is false the whole row is automatically not full.
            if (grid[x, y] == null)
            {
                return false;
            }

        }
        /// Since we found a complete row, we incriment the number of rows popped.
        currentPoppedRows ++;
        return true;
    }

    public void deleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void moveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }

    }
    public void moveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            moveRowDown(i);
        }
    }
    public void deleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (isFullAt(y))
            {
                deleteMinoAt(y);
                moveAllRowsDown(y + 1);
                --y;
            }

        }
    }

    public void updateGrid(Tetrimino tetrimino)
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetrimino.transform)
                    {
                        grid[x, y] = null;
                    }

                }
            }
        }
        foreach (Transform mino in tetrimino.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }






    /// <summary>
    /// Determins whether or not the 2d vector (which is tied to a tetrimino) is within bounds of the grid.
    /// </summary>
    /// <param name="pos"></param> -- the 2d vector we are checking is winthin bounds.
    /// <returns></returns> -- return true if the 2d vector is within bounds of grid, elsewise 2d is out of bounds.
    public bool inGrid(Vector2 pos)
    {
        //confirms tetrimino is within bounds of the x-plane axis, and the y plane axis.
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }




    /// <summary>
    /// Rounds every portion of a 2d vector.
    /// </summary>
    /// <param name="pos"></param>  -- the given 2d vector we want to round.
    /// <returns></returns>  the same 2d vector pos, but rounded.
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    ///Instantiates a prefab within the game.
    public void spawnNextTetrimino()
    {

        GameObject nextTetrimino = (GameObject)Instantiate(Resources.Load(getRandomTetrimino(), typeof(GameObject)), new Vector2(5.0f, 19.0f), Quaternion.identity);
    }

    /// <summary>
    /// Generates a prefab directory.
    /// </summary>
    /// <returns>
    /// Returns a string that contains the prefab directory of a random tetrimino.
    /// </returns>
    string getRandomTetrimino()
    {
        int randomTetrimino = Random.Range(1, 8); // 1 to 7
        string randomTetriminoName = "";
        switch (randomTetrimino)
        {
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
