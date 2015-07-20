using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHUDController : MonoBehaviour {

	public static GameHUDController instance;

	[SerializeField] private Text m_scoreLbl;
	[SerializeField] private HealthBar m_healthBar;
	[SerializeField] private GameObject m_pauseBtn;
	[SerializeField] private GameObject m_resumeBtn;
	[SerializeField] private GameObject m_gameOverScreen;

	void Start() {
		instance = this;
		Init();
	}

	private void Init() {
		m_scoreLbl.text = "0";
		m_healthBar.UpdateHealthBar (100f, 100f);
		m_pauseBtn.SetActive(true);
		m_resumeBtn.SetActive(false);
		m_gameOverScreen.SetActive(false);
	}

	public void SetScore(int p_score) {
		m_scoreLbl.text = p_score.ToString();
	}

	public void UpdateHealthBar(float p_currentHp, float p_maxHp) {
		m_healthBar.UpdateHealthBar(p_currentHp, p_maxHp);
	}

	public void Pause() {
		if(m_pauseBtn.activeSelf) {
			m_pauseBtn.SetActive(false);
			m_resumeBtn.SetActive(true);
			GameController.instance.ChangeState(GameState.paused);
		}
	}

	public void Resume() {
		if(m_resumeBtn.activeSelf) {
			m_pauseBtn.SetActive(true);
			m_resumeBtn.SetActive(false);
			GameController.instance.Resume();
		}
	}

	public void GameOver(int p_score, int p_highScore) {
		m_pauseBtn.SetActive(false);
		m_gameOverScreen.GetComponent<GameOverScreen>().SetScore(p_score, p_highScore);
		StartCoroutine("ShowGameOverScreen");
	}

	private IEnumerator ShowGameOverScreen() {
		yield return new WaitForSeconds(1.5f);
		m_gameOverScreen.SetActive(true);
	}

	public void RestartGame() {
		Init();
	}
}
