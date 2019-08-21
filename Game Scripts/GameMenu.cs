using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMenu : MonoBehaviour {
    public Text levelText;
    // Start is called before the first frame update
    void Start() {
        // Sets the difficulty level at 0.
        levelText.text = "0";
    }

    // No need for update

    public void playGame() {
        // If the difficulty level text changes from 0
        if (levelText.text != "0") {
            // Then the game did not start at 0
            Game.zeroStart = false;
            // We change the starting level to the correct difficulty.
            Game.startingLevel = int.Parse(levelText.text);
        }
        else {
            // Otherwise the game did start at 0 and the difficulty is at 0.
            Game.zeroStart = true;
            Game.startingLevel = 0;

        }
        // Changes screen to the level.
        SceneManager.LoadScene("level");
    }

    public void changeVal(float value) {
        // Changes the value of the difficulty previewer.
        levelText.text = value.ToString();

    }
}
