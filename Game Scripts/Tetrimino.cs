using UnityEngine;
using System.Collections;

public class Tetrimino : MonoBehaviour {
    //                                                                  G l o b a l    V a r i a b l e s.

    float fall = 0; // Timer for fallSpeed

    private float fallSpeed; // The number of seconds needed for a tetrmino to go down by one unit on the y axis.
    public bool allowRotation = true; // Public bool to let us determine whether our prefabs allow roation.
    public bool limitRotation = false; // Public bool to let us determine if our prefabs are limited in rotation.

    public int indivisualScore = 100; // Maximum points for immidiately putting a block down
    public float indivisualScoreTime; // Score value for the current tetrimino. (Variable name pending)

    private float constantHorizontalSpeed = 0.06f; // The translation speed of a tetrimino on the y-axis.
    private float constantVerticalSpeed = 0.1f; // The translation speed of a tetrimino on the x-axis.
    private float downwardWaitMax = 0.02f; // Wait time for the game to a button-holdown.


    private float verticalTimer = 0; 
    private float horizontalTimer = 0;
    private float downwardWaitTimerHorizontal= 0;
    private float downwardWaitTimerVertical = 0;

    private bool horizontalImmidiateMovement = false; // Determines if a key was being held horizontally.
    private bool verticalImmidiateMovement = false; // Determines if a key was being held vertically.



    // Sound references
    public AudioClip moveSound;    // Container that stores our clear line sound audio clip.
    public AudioClip rotateSound;  // Container that stores our rotate sound audio clip.
    public AudioClip landSound;    // Container that stores our landing sound audio clip.
    private AudioSource audioSource; // The built-in method that allows us to play sounds.




    //                                                          G a m e    F u n c t i o n a l i t y    P o r t o n.

    // Start is called before the first frame update
    void Start() {
        // Creates the ability to play music whenever I'd like to.
        audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame.
    void Update() {

        checkUserInput(); // Checks input (arrow keys) and modifies a tetrimino's position.
        updateIndivisualScore();
        updateFallSpeed();
    }


    //                                                                   M o v e m e n t    P o r t i o n.

    void checkUserInput() {
        // Acts upon arrow-key input, being able to maniplulate the position / rotation of a tetrimino.
        // Works with the arror keys 
        // Left -- Moves the tetrimino to the left by one 'block'
        // Right -- Moves the tetrimino to the right by one 'block'
        // Up -- Rotates the tetrimino by 90 degrees to the right.
        // Down - translates the tetrimino down by one row.


        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            horizontalImmidiateMovement = false;
            horizontalTimer = 0;
            downwardWaitTimerHorizontal = 0;
        }
        if(Input.GetKeyUp(KeyCode.DownArrow)) {
            verticalImmidiateMovement = false;
            verticalTimer = 0;
            downwardWaitTimerVertical = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {

            moveRight();
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {

            moveLeft();
        }
         if (Input.GetKeyDown(KeyCode.UpArrow)) {

            moveUp();
        }
         if (Input.GetKey(KeyCode.DownArrow) || (Time.time - fall >= fallSpeed)) {

            moveDown();
        }
    }

    bool isValidPosition() {
        // Confirms the tetrimino is not on another mino or out of bounds.
        foreach (Transform mino in transform) {

            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().inGrid(pos) == false)  {

                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform) {

                return false;
            }
        }
        return true;
    }

    void updateIndivisualScore() {

        if (indivisualScoreTime < 1) {

            indivisualScoreTime += Time.deltaTime;
        }
        else {

            indivisualScoreTime = 0;
            indivisualScore = Mathf.Max(indivisualScore - 10, 0);
        }
    }





    void moveRight() {
        if (horizontalImmidiateMovement) {

            if (downwardWaitTimerHorizontal < downwardWaitMax) {

                downwardWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < constantHorizontalSpeed) {

                horizontalTimer += Time.deltaTime;
                return; // Exits method
            }
        }
        if (!horizontalImmidiateMovement) {

            horizontalImmidiateMovement = true;
        }
        horizontalTimer = 0;
        // Moves tetrimino to the right
        transform.position += Vector3.right;
        // Confirms the tetrimino is not out of bounds or on another tetrimino.
        if (isValidPosition()) {
        
            // Fills the grid in the correct mino location. Also plays a sound.
            FindObjectOfType<Game>().updateGrid(this);
            play_move_Audio();
        }
        else{

            transform.position += Vector3.left;
        }
    }

    void moveLeft() {
        if (horizontalImmidiateMovement) {

            if (downwardWaitTimerHorizontal < downwardWaitMax) {

                downwardWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < constantHorizontalSpeed) {

                horizontalTimer += Time.deltaTime;
                return; // Exits method
            }
        }
        if (!horizontalImmidiateMovement) {

            horizontalImmidiateMovement = true;
        }
        horizontalTimer = 0;
        transform.position += Vector3.left;
        if (isValidPosition()) {

            FindObjectOfType<Game>().updateGrid(this);
            play_move_Audio();
        }
        else {
            transform.position += Vector3.right;
        }
    }

    void moveUp() {

        if (allowRotation) {

            if (limitRotation) {

                if (transform.rotation.eulerAngles.z >= 90) {

                    transform.Rotate(0, 0, -90);
                }
                else {

                    transform.Rotate(0, 0, 90);
                }
            }
            else {

                transform.Rotate(0, 0, 90);
            }
        }

        if (isValidPosition()) {

            FindObjectOfType<Game>().updateGrid(this);
            if (allowRotation) {

                play_rotate_Audio();
            }

        }
        else {
            if (limitRotation) {

                if (transform.rotation.eulerAngles.z >= 90) {

                    transform.Rotate(0, 0, -90);
                }
                else {

                    transform.Rotate(0, 0, 90);
                }
            }
            else {
                transform.Rotate(0, 0, -90);
            }

        }
    }

    void moveDown() {

        if (verticalImmidiateMovement) {

            if (downwardWaitTimerVertical < downwardWaitMax) {

                downwardWaitTimerVertical+= Time.deltaTime;
                return;
            }
            if (verticalTimer < constantVerticalSpeed) {

                verticalTimer += Time.deltaTime;
                return;
            }
        }
        if (!verticalImmidiateMovement) {

            verticalImmidiateMovement = true;
        }

        verticalTimer = 0;
        transform.position += Vector3.down;
        if (isValidPosition()) {

            if (Input.GetKey(KeyCode.DownArrow)) {

                play_move_Audio();
            }
            FindObjectOfType<Game>().updateGrid(this);

        }
        else {

            transform.position += Vector3.up;
            FindObjectOfType<Game>().deleteRow();
            if (FindObjectOfType<Game>().checkIsAboveGrid(this)) {

                FindObjectOfType<Game>().gameOver();
            }
            Game.currentScore += indivisualScore;
            enabled = false;
            FindObjectOfType<Game>().spawnNextTetrimino();
            play_land_Audio();
        }
        fall = Time.time;
    }
    void updateFallSpeed() {
        // Copies the slider fall speed to the fall speed variable.
        fallSpeed = Game.fallSpeed;
    }



    //                                                                   S o u n d    P o r t i o n.

    void play_move_Audio() {

        audioSource.PlayOneShot(moveSound); // Plays the move sound audio clip.
    }
    void play_land_Audio() {

        audioSource.PlayOneShot(landSound); // Plays the land sound audio clip.
    }
    void play_rotate_Audio() {

        audioSource.PlayOneShot(rotateSound); // Plays the rotate sound audio clip.
    }


    
}



