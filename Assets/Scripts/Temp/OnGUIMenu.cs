using UnityEngine;
using System.Collections;

public class OnGUIMenu : MonoBehaviour {

	public static OnGUIMenu instance;

	public int playerScore = 0;
	public bool isGameOver = false;

	void Start () {
		instance = this;
	}

	void OnGUI() {
		GUI.Label(new Rect(Screen.width - 100, Screen.height - 25, 200, 100), "Score: " + playerScore);

		if(isGameOver) {
			GUI.Label(new Rect((Screen.width / 2) - 50, Screen.height / 2 - 50, 200, 100), "GAMEOVER!!!");
			GUI.Label(new Rect((Screen.width / 2) - 35, Screen.height / 2 - 25, 200, 100), "Score: " + playerScore);
			if(GUI.Button(new Rect((Screen.width / 2) - 100, Screen.height / 2, 200, 100), "RESTART GAME")) {
				playerScore = 0;
				isGameOver = false;
				LevelController.instance.RestartGame();
			}
		}
	}
}
