using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public static MainMenuController instance;

	void Start() {
		instance = this;
	}

	public void StartGame() {
		GameController.instance.StartCoroutine("LoadLevel");
	}

	public void OpenSettings() {
		Debug.Log("Settings");
	}
}
