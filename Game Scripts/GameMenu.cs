using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMenu : MonoBehaviour {
    public Text levelText;
    // Start is called before the first frame update
    void Start() {
        
        levelText.text = "0";
    }

    // No need for update

    public void playGame() {
        if (levelText.text != "0") {
            Game.zeroStart = false;
            Game.startingLevel = int.Parse(levelText.text);
        }
        else {
            Game.zeroStart = true;
            Game.startingLevel = 0;

        }
        SceneManager.LoadScene("level");
    }

    public void changeVal(float value) {
        levelText.text = value.ToString();

    }
}
