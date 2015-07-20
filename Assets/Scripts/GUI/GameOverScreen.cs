using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	public static GameOverScreen instance;

	[SerializeField] private Text m_scoreLbl;
	[SerializeField] private Text m_highScoreLbl;

	void Start() {
		instance = this;
	}

	public void SetScore(int p_score, int p_highScore) {
		m_scoreLbl.text = "Score: " + p_score.ToString();
		m_highScoreLbl.text = "High Score: " + p_highScore.ToString();
	}

	public void RestartGame() {
		GameController.instance.ChangeState(GameState.restartLevel);
	}

	public void ReturnToMain() {
		GameController.instance.ChangeState(GameState.main);
	}
}
