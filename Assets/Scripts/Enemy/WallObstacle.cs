using UnityEngine;
using System.Collections;

public class WallObstacle : MonoBehaviour {

	[SerializeField] private int m_hitpoints = 10;

	public int currentHitpoints = default(int);

	void OnEnable() {
		Init();
	}

	private void Init() {
		currentHitpoints = m_hitpoints;
	}

	public void Hit() {
		currentHitpoints--;
		GameController.instance.AddScore(2);
		if(currentHitpoints <= 0) {
			GameController.instance.AddScore(50);
			this.gameObject.Recycle();
		}
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "PlayerProjectile")	{
			Hit();
		}
	}
}
