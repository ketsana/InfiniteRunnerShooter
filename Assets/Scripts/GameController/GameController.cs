using UnityEngine;
using System.Collections;

public enum GameState {
	main = 0,
	playing,
	paused,
	gameOver,
	restartLevel,
}

public class GameController : MonoBehaviour {

	public static GameController instance { get; private set; }

	[SerializeField] private GameObject m_playerPrefab;

	private bool isStateTransition = false;
	private float m_timeScale = 1f;

	public GameState gameState = GameState.playing;
	public PlayerController playerController;
	public int highScore;
	public int playerScore;
	public int coins;
	public bool isPaused = false;

	public int HighScore {
		get { 
			if(PlayerPrefs.HasKey("HighScore")) {
				highScore = PlayerPrefs.GetInt("HighScore");
				return  highScore;
			} else {
				highScore = 0;
				PlayerPrefs.SetInt("HighScore", 0);
				PlayerPrefs.Save();
				return 0;
			}
		}
		set {
			highScore = value;
			PlayerPrefs.SetInt("HighScore", value);
			PlayerPrefs.Save();
		}
	}

	public int Coins {
		get { 
			if(PlayerPrefs.HasKey("Coins")) {
				coins = PlayerPrefs.GetInt("Coins");
				return  coins;
			} else {
				coins = 0;
				PlayerPrefs.SetInt("Coins", 0);
				PlayerPrefs.Save();
				return 0;
			}
		}
		set {
			coins = value;
			PlayerPrefs.SetInt("Coins", value);
			PlayerPrefs.Save();
		}
	}

	void Awake() {
		if(instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		Init ();
	}

	private void Init() {
		highScore = HighScore;
//		ChangeState(GameState.main);
	}

//	void Update() {
//		ProcessGameState();
//	}

	private void LoadMainScene() {
		if(!Application.loadedLevelName.Equals("MainScene", System.StringComparison.Ordinal)) {
			Application.LoadLevel("MainScene");
		}
	}

	private void ProcessGameState() {
		switch(gameState) {
		case GameState.main:
			if(isStateTransition) {
				isStateTransition = false;
				LoadMainScene();
			}
			break;
		case GameState.playing:
			if(isStateTransition) {
				isStateTransition = false;
			}
			break;
		case GameState.paused:
			if(isStateTransition) {
				isStateTransition = false;
				Pause();
			}
			break;
		case GameState.gameOver:
			if(isStateTransition) {
				isStateTransition = false;
				GameOver();
			}
			break;
		case GameState.restartLevel:
			if(isStateTransition) {
				isStateTransition = false;
				RestartLevel();
			}
			break;
		}
	}

	public void ChangeState(GameState p_newState) {
		isStateTransition = true;
		gameState = p_newState;
		ProcessGameState();
	}

	public void Pause() {
		if(!isPaused) {
			isPaused = true;
			m_timeScale = Time.timeScale;
			Time.timeScale = 0f;
		}
	}

	public void Resume() {
		if(isPaused) {
			isPaused = false;
			Time.timeScale = m_timeScale;
			ChangeState(GameState.playing);
		}
	}

	public void AddScore(int p_score) {
		playerScore += p_score;
		GameHUDController.instance.SetScore(playerScore);
	}

	public void AddCoins(int p_coins) {
		coins += p_coins;
		//TODO set coins in gamehud
//		GameHUDController.instance.SetCoins(coins);
	}

	public void GameOver() {
		HighScore = (playerScore > HighScore) ? playerScore : HighScore;
		GameHUDController.instance.GameOver(playerScore, HighScore);
		Coins = coins;
		//TODO show all coins collected in gameover screen
	}

	public IEnumerator LoadLevel() {
		Application.LoadLevel("GameScene");
		yield return null;
		RestartLevel();
	}

	private void RestartLevel() {
		Destroy(GameObject.Find("Player"));
		ObjectPool.RecycleAll ();
		GameHUDController.instance.RestartGame();
		LevelController.instance.RestartGame();
		GameObject _player = (GameObject)Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
		_player.name = "Player";
		playerController = _player.GetComponent<PlayerController>();
		playerScore = 0;
		ChangeState(GameState.playing);
	}
}
