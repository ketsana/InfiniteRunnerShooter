using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerVO {
	public float currentHp;
	public float maxHp;
	public float forwardSpeed = 5f;
	public float sideSpeed = 4f;
	public GameObject[] projectilePrefabs;
}

public enum PlayerState {
	idle = 0,
	run,
	death,
}

public enum ProjectileType {
	normal = 0,
	trippleFireball,
}

public class PlayerController : MonoBehaviour {
	
	public PlayerVO playerVO;
	public PlayerState state = PlayerState.idle;
	public bool allowMoveForward = false;
	public bool allowMoveSideStep = false;
	public bool allowFire = false;
	public bool isGameOver = false;
	public ProjectileType projectileType = ProjectileType.normal;
	public float projectileTimer = 0;
	
	[SerializeField] private Animator m_animatorController;

	private bool isStateTransition = false;
	private PlayerState m_previousState;
	private float m_leftLimit;
	private float m_rightLimit;
	private float m_speedOnDeath;
	private float[] m_xPositions;
	private int m_currentPosColumn;
	private int m_previousPosColumn;
	private MouseTouchControls m_mouseTouchControls;

	void Start () {
		Init();
		StartCoroutine("StartGame");
	}

	private void Init() {
		m_leftLimit = LevelController.instance.leftLimit;
		m_rightLimit = LevelController.instance.rightLimit;
		allowMoveForward = false;
		allowMoveSideStep = false;
		allowFire = false;	
		isGameOver = false;
		playerVO.currentHp = playerVO.maxHp;
		m_animatorController.SetBool("Death", false);
		this.transform.position = LevelController.instance.PlayerStartPosition;
		m_xPositions = LevelController.instance.GetWallPositionsX();
		m_currentPosColumn = Mathf.CeilToInt(m_xPositions.Length / 2);
		m_previousPosColumn = m_currentPosColumn;
		m_mouseTouchControls = this.GetComponent<MouseTouchControls>();
	}

	private IEnumerator StartGame() {
		yield return new WaitForSeconds(2f);
		allowMoveForward = true;
		allowMoveSideStep = true;
		allowFire = true;
		ChangeState(PlayerState.run);
	}

	void Update() {
		ProcessState();
		ProcessProjectileTimer();
	}

	private void ProcessState() {
		switch(state) {
		case PlayerState.idle:
			if(isStateTransition) {
				isStateTransition = false;
				m_animatorController.SetTrigger("Idle");
			}
			break;
		case PlayerState.run:
			if(isStateTransition) {
				isStateTransition = false;
				m_animatorController.SetTrigger("Run");
			}
			if(!GameController.instance.isPaused){
				if(allowMoveForward) this.transform.Translate(Vector3.forward * playerVO.forwardSpeed * Time.deltaTime);
#if UNITY_EDITOR
				KeyBoardControls();
#else
				MobileControls();
#endif
			}
			break;
		case PlayerState.death:
			if(isStateTransition) {
				isStateTransition = false;
				m_animatorController.SetBool("Death", true);
				isGameOver = true;
				allowMoveForward = false;
				allowMoveSideStep = false;
				allowFire = false;
				m_speedOnDeath = playerVO.forwardSpeed;
			}
			SlideOnDeath();
			break;
		}
	}

	private void ProcessProjectileTimer() {
		if(projectileTimer == 0) {
			return;
		} else {
			projectileTimer -= Time.deltaTime;
		}

		if(projectileTimer <= 0) {
			projectileTimer = 0;
			projectileType = ProjectileType.normal;
		}
	}

	private void KeyBoardControls() {
		if(allowMoveSideStep) {
//			//sideSpeed = 4
//			if(this.transform.position.x > m_leftLimit && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) {
//				this.transform.Translate(Vector3.left * playerVO.sideSpeed * Time.deltaTime);
//			}
//			if(this.transform.position.x < m_rightLimit && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) {
//				this.transform.Translate(Vector3.right * playerVO.sideSpeed * Time.deltaTime);
//			}

			// Move Left Trigger
			if(m_currentPosColumn != 0 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))) {
				m_previousPosColumn = m_currentPosColumn;
				m_currentPosColumn--;
			}
			// Move Right Trigger
			if(m_currentPosColumn != (m_xPositions.Length - 1) && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))) {
				m_previousPosColumn = m_currentPosColumn;
				m_currentPosColumn++;
			}

			// Update column movement
			if(m_currentPosColumn <  m_previousPosColumn) {
				if(this.transform.position.x > m_xPositions[m_currentPosColumn]) {
					this.transform.Translate(Vector3.left * playerVO.sideSpeed * Time.deltaTime);
				}
			} else if (m_currentPosColumn > m_previousPosColumn) {
				if(this.transform.position.x < m_xPositions[m_currentPosColumn]) {
					this.transform.Translate(Vector3.right * playerVO.sideSpeed * Time.deltaTime);
				}
			}			
		}
		
		if(allowFire) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				SpawnProjectile();
			}
		}
	}

	private void MobileControls() {
		if(allowMoveSideStep) {
			// Move Left Trigger
			if(m_currentPosColumn != 0 && m_mouseTouchControls.currentMouseTouchState == MouseTouchControls.MouseTouchState.MOUSE_TOUCH_SWIPE_LEFT) {
				m_mouseTouchControls.currentMouseTouchState = MouseTouchControls.MouseTouchState.DEFAULT;
				m_previousPosColumn = m_currentPosColumn;
				m_currentPosColumn--;
			}
			// Move Right Trigger
			if(m_currentPosColumn != (m_xPositions.Length - 1) && m_mouseTouchControls.currentMouseTouchState == MouseTouchControls.MouseTouchState.MOUSE_TOUCH_SWIPE_RIGHT) {
				m_mouseTouchControls.currentMouseTouchState = MouseTouchControls.MouseTouchState.DEFAULT;
				m_previousPosColumn = m_currentPosColumn;
				m_currentPosColumn++;
			}
			
			// Update column movement
			if(m_currentPosColumn <  m_previousPosColumn) {
				if(this.transform.position.x > m_xPositions[m_currentPosColumn]) {
					this.transform.Translate(Vector3.left * playerVO.sideSpeed * Time.deltaTime);
				}
			} else if (m_currentPosColumn > m_previousPosColumn) {
				if(this.transform.position.x < m_xPositions[m_currentPosColumn]) {
					this.transform.Translate(Vector3.right * playerVO.sideSpeed * Time.deltaTime);
				}
			}			
		}
		
		if(allowFire) {
			if(m_mouseTouchControls.currentMouseTouchState == MouseTouchControls.MouseTouchState.MOUSE_TOUCH_TAP) {
				m_mouseTouchControls.currentMouseTouchState = MouseTouchControls.MouseTouchState.DEFAULT;
				SpawnProjectile();
			}
		}
	}

	private void SpawnProjectile() {
		Vector3 _pos = this.transform.position;
		_pos.z += 1;
		switch(projectileType) {
		case ProjectileType.normal:
			playerVO.projectilePrefabs[0].Spawn(_pos, Quaternion.identity);
			break;
		case ProjectileType.trippleFireball:
			playerVO.projectilePrefabs[0].Spawn(_pos, Quaternion.identity);
			playerVO.projectilePrefabs[1].Spawn(_pos, Quaternion.identity);
			playerVO.projectilePrefabs[2].Spawn(_pos, Quaternion.identity);
			break;
		}
	}

	public void SetTimedProjectile(ProjectileType p_type, float p_timer) {
		projectileType = p_type;
		projectileTimer = p_timer;
	}

	private void SlideOnDeath() {
		if(m_speedOnDeath > 0) {
			m_speedOnDeath = Mathf.Lerp(m_speedOnDeath, 0, Time.deltaTime * 0.5f);
			this.transform.Translate(Vector3.forward * m_speedOnDeath * Time.deltaTime);
		}
	}

	private void ChangeState(PlayerState p_state) {
		m_previousState = state;
		state = p_state;
		isStateTransition = true;
	}

	private void TakeDamage(float p_amount) {
		if(state ==  PlayerState.death) return;
		playerVO.currentHp -= p_amount;
		GameHUDController.instance.UpdateHealthBar(playerVO.currentHp, playerVO.maxHp);
		if(playerVO.currentHp <= 0f) {
			ChangeState(PlayerState.death);
			GameController.instance.ChangeState(GameState.gameOver);
		}
	}

	void OnTriggerEnter (Collider collider)	{
//		Debug.Log (collider.tag);
		if(state ==  PlayerState.death) return;
		if (collider.tag == "Obstacle") {
			TakeDamage(50f);
		} else if (collider.tag == "MinionEnemy") {
			TakeDamage(15f);
		} else if (collider.name == Constants.COIN) {
			collider.gameObject.Recycle();
			Debug.Log("Add coins");
			//TODO add coins
		} else if (collider.name == Constants.POWERUP_TRIPPLE_FIREBALL) {
			SetTimedProjectile(ProjectileType.trippleFireball, 10);
		}
	}

//	public void RestartGame() {
//		allowMoveForward = false;
//		allowMoveSideStep = false;
//		allowFire = false;	
//		isGameOver = false;
//		m_animatorController.SetBool("Death", false);
//		ChangeState(PlayerState.idle);
//		this.transform.position = LevelController.instance.PlayerStartPosition;
//		StartCoroutine("StartGame");
//	}
}
