using UnityEngine;
using System.Collections;

public class Tetrimino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkUserInput();
    }


    //Constantly gathers User Input. Acts upon arrow-key input.
    void checkUserInput() {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (isValidPosition())
            {

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

            }
            else
            {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - fall >= fallSpeed))
        {
            transform.position += Vector3.down;
            if (isValidPosition())
            {

            }
            else
            {
                transform.position += Vector3.up;
                enabled = false;
                FindObjectOfType<Game>().spawnNextTetrimino();
            }
            fall = Time.time;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation) {
                if (limitRotation) {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
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

            }
            else {
                if (limitRotation) {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else {
                    transform.Rotate(0, 0, -90);
                }

            }
        }
    }

    bool isValidPosition() {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().inGrid(pos) == false)
            {
                return false;
            }

        }
        return true;
    }
}
