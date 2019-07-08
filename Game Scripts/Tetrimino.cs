using UnityEngine;
using System.Collections;

public class Tetrimino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;

    public int indivisualScore = 100; //Maximum points for immidiately putting a block down
    public float indivisualScoreTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkUserInput();
        updateIndivisualScore();
    }


    //Constantly gathers User Input. Acts upon arrow-key input.
    void checkUserInput()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (isValidPosition())
            {
                FindObjectOfType<Game>().updateGrid(this);
            }
            else
            {
                transform.position += Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if (isValidPosition())
            {
                FindObjectOfType<Game>().updateGrid(this);
            }
            else
            {
                transform.position += Vector3.right;
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }

                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }

            if (isValidPosition())
            {
                FindObjectOfType<Game>().updateGrid(this);
            }
            else
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - fall >= fallSpeed))
        {
            transform.position += Vector3.down;
            if (isValidPosition())
            {
                FindObjectOfType<Game>().updateGrid(this);
            }
            else
            {
                transform.position += Vector3.up;
                FindObjectOfType<Game>().deleteRow();
                if (FindObjectOfType<Game>().checkIsAboveGrid(this)) {
                    FindObjectOfType<Game>().gameOver();
                }
                Game.currentScore += indivisualScore;
                enabled = false;
                FindObjectOfType<Game>().spawnNextTetrimino();
            }
            fall = Time.time;

        }
    }

    bool isValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().inGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
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
}
