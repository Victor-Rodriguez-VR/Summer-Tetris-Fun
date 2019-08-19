using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game : MonoBehaviour
{   // classic tetris games always have width set to 10, and height set to 20.
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    public int[] scores = new int[] { 40, 100, 300, 1200};
    public int currentPoppedRows = 0;
    public static int currentScore = 0;
    public float fallSpeed = 1.0f;

    public int currentLevel = 0;    // The 'current level' of difficulty.
    private int num_Of_Lines_Cleared = 0;   // The number of lines cleared over the duration of the whole game.

    private AudioSource audioSource; // Gives us the ability to play sounds from this script.
    public AudioClip clearLineSound; // Public varaible that we store our clear line sound audio clip.


    private GameObject previewTetrimino; // Is the upcoming tetrimino block once the player lands the nextTetrimino object (which is the tetrimino they control)
    private GameObject nextTetrimino;   // The tetrimino the user controlls.

    private bool gameStarted = false; // Determines whether the game has started or not.
    private Vector2 previewPosition = new Vector2(-5.5f, 15.0f); // The spawn location of the nextTetrimino

    public Text huddyHUD;   // Total score variable ( variable name pending ) 
    public Text hud_level;  // Current level variable.
    public Text hud_lines;  // # of lines cleared variable.
    void Start() {
        audioSource = GetComponent<AudioSource>();
        spawnNextTetrimino();
    }
    
    void Update() {

        updateScore();
        updateUI();
        updateLevel();
        updateSpeed();
    }
    public void updateScore() {
        
        if (currentPoppedRows > 0) {
            
            currentScore += scores[currentPoppedRows - 1] + (currentLevel+20);
            num_Of_Lines_Cleared += currentPoppedRows;
            play_LineCleared_Audio();
        }
        currentPoppedRows = 0;


    }
    /// <summary>
    /// Updates the score on the screen.
    /// </summary>
    public void updateUI() {
        huddyHUD.text = currentScore.ToString();
        hud_level.text = currentLevel.ToString();
        hud_lines.text = num_Of_Lines_Cleared.ToString();
    }




    /// <summary>
    /// Determins whether or not a tetrimino is within the game grid. 
    /// </summary>
    /// <param name="tetrimino">
    /// The current tetrimino in use.
    /// </param>  
    /// <returns>
    /// True - tetrimino is in the grid, otherwise false - the tetrimino is above the grid.
    /// </returns>
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

    /// <summary>
    /// Moves user to the game over screen.
    /// </summary>
    public void gameOver() {
        SceneManager.LoadScene("GameOver");
        

    }

    /// <summary>
    /// Confirms the tetrimino is not above the grid
    /// </summary>
    /// <param name="pos">
    /// The latest position of a tetrimino block.
    /// </param>
    /// <returns>
    /// An updated grid with the blocks being accounted for, or null.
    /// </returns>
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1) {
            return null;
        }
        else {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    /// <summary>
    /// Confirms whether or not a row is full within the grid.
    /// </summary>
    /// <param name="y">
    /// The y-axis value of the grid. 
    /// </param>
    /// <returns>
    /// True - the row is full, otherwise false and the row is not full.
    /// </returns>
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


    /// <summary>
    /// Deletes a row of minos.
    /// </summary>
    /// <param name="y">
    /// The row of minos you want to delete. 
    /// </param>
    public void deleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }



    /// <summary>
    /// Translates every non-null item within the row down by one on the y-axis.
    /// </summary>
    /// <param name="y">
    /// The y-axis (row) of which you want to translate downward.
    /// </param>
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

    /// <summary>
    /// Translates every non-null item within the grid down by one.
    /// </summary>
    /// <param name="y">
    /// The y-axis (row) of which you want to translate downward.
    /// </param>
    public void moveAllRowsDown(int y) {
        for (int i = y; i < gridHeight; ++i) {
            moveRowDown(i);
        }
    }

    /// <summary>
    /// Deletes every mino within a row, and translates every row down by one.
    /// </summary>
    public void deleteRow() {
        for (int y = 0; y < gridHeight; ++y) {
            if (isFullAt(y)) {
                deleteMinoAt(y);
                moveAllRowsDown(y + 1);
                --y;
            }

        }
    }
    /// <summary>
    /// Fills the grid in accordance with the tetrimino's position.
    /// </summary>
    /// <param name="tetrimino"></param>
    public void updateGrid(Tetrimino tetrimino)
    {
        for (int y = 0; y < gridHeight; ++y) {
            for (int x = 0; x < gridWidth; ++x) {
                if (grid[x, y] != null)  {
                    if (grid[x, y].parent == tetrimino.transform) {
                        grid[x, y] = null;
                    }

                }
            }
        }
        foreach (Transform mino in tetrimino.transform) {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight) {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }






    /// <summary>
    /// Determines whether or not the position of a tetrimino is within bounds of the grid.
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
    public void spawnNextTetrimino() {

        if (!gameStarted) {
            // One-time use.
            gameStarted = true;
            // Instantiates nextTetrimino (user controlled tetrimino)
            nextTetrimino = (GameObject)Instantiate(Resources.Load(getRandomTetrimino(), typeof(GameObject)), new Vector2(5.0f, 19.0f), Quaternion.identity);
            // Instantiates previewTetrimino (the tetrimino that appears on screen saying its next)
            previewTetrimino = (GameObject)Instantiate(Resources.Load(getRandomTetrimino(), typeof(GameObject)), previewPosition, Quaternion.identity);
            // Disables preview tetrimino so user movement does not affect the block's position.
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
        }
        else {
            // Moves previewTetrimino to the spawning posiiton.
            previewTetrimino.transform.localPosition = new Vector2(5.0f, 20.0f);
            // nextTetrimino becomes previewTetrimino
            nextTetrimino = previewTetrimino;
            // Re-enabled user-movement.
            nextTetrimino.GetComponent<Tetrimino>().enabled = true;
            // Generates a new instance of a tetrimino, and moves it to the previewPositon.
            previewTetrimino = (GameObject)Instantiate(Resources.Load(getRandomTetrimino(), typeof(GameObject)), previewPosition, Quaternion.identity);
            // Disables movement for the newly created tetrimino. It is only for viewing purposes (till the new time spawnNextTetrimino is called)
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;


        }
        
    }

    /// <summary>
    /// Generates a prefab directory.
    /// </summary>
    /// <returns>
    /// Returns a string that contains the prefab directory of a random tetrimino.
    /// </returns>
    string getRandomTetrimino() {
        int randomTetrimino = Random.Range(1, 8); // 1 to 7
        string randomTetriminoName = "";
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

    //                                                             S o u n d     P o r t i o n.
    void play_LineCleared_Audio() {
        audioSource.PlayOneShot(clearLineSound); //// Plays the clear line sound audio clip.
    }





    //                                                               D i f f i c u l t y    P o r t i o n.


    void updateLevel() {
        // Changes the current level if the total number of lines cleared is greater than 10.
        currentLevel = num_Of_Lines_Cleared / 10;
    }
    void updateSpeed() {
        // Reduces the fall speed .1f based on the current level.
        fallSpeed = 1.0f - ((float) currentLevel * .1f);
    }
}
