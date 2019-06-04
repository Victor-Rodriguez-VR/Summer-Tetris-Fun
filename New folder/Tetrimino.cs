using UnityEngine;
using System.Collections;

public class Tetrimino : MonoBehaviour { 

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

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            transform.position += Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.position += Vector3.up;
        }
    }
}
