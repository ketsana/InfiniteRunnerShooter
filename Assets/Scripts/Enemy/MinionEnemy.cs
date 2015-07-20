using UnityEngine;
using System.Collections;

public class MinionEnemy : MonoBehaviour {

	[SerializeField] private int m_hitpoints = 1;
	[SerializeField] private GameObject m_deathSprite = default(GameObject);
	[SerializeField] private Drop[] m_drops = default(Drop[]);

	public MinionEnemyState state;
	public int currentHitpoints = default(int);

	private bool isStateTransition = false;

	void OnEnable() {
		Init ();
	}

	private void Init() {
		ChangeState(MinionEnemyState.idle);
		this.renderer.enabled = true;
		m_deathSprite.SetActive(false);
		this.collider.enabled = true;
		currentHitpoints = m_hitpoints;
	}

	void Update() {
		ProcessState();
	}

	private void ProcessState() {
		switch(state) {
		case MinionEnemyState.idle:
			break;
		case MinionEnemyState.walk:
			break;
		case MinionEnemyState.death:
			if(isStateTransition) {
				isStateTransition = false;
				StartCoroutine("Death");
			}
			break;
		}
	}

	private void ChangeState(MinionEnemyState p_state) {
		state = p_state;
		isStateTransition = true;
	}

	private IEnumerator Death() {
		this.renderer.enabled = false;
		m_deathSprite.SetActive(true);
		this.collider.enabled = false;
		yield return new WaitForSeconds(0.4f);

		RandomDrop();
		yield return new WaitForSeconds(0.1f);

		this.gameObject.Recycle();
	}

	private void RandomDrop() {
		int _randomInt = Random.Range(0, 100);
		int _dropChance = 0;
		foreach(Drop drop in m_drops) {
			_dropChance += drop.chance;
			if(_randomInt <= _dropChance) {
				GameObject _dropLoot = drop.objectPrefab.Spawn(this.transform.position, Quaternion.identity);
				_dropLoot.name = drop.objectPrefab.name;
				break;
			}
		}
	}

	public void Hit() {
		if(state != MinionEnemyState.death) {
			currentHitpoints--;
			GameController.instance.AddScore(5);
			if(currentHitpoints <= 0) {
				GameController.instance.AddScore(10);
				ChangeState(MinionEnemyState.death);
			}
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "PlayerProjectile")	{
			Hit();
		}
	}
}
