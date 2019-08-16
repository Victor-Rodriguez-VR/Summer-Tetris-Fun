using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenSYS : MonoBehaviour {

    public void playAgain() {
        // Changes the screen of to the game level that holds the grid.
        SceneManager.LoadScene("level");
    }
}
