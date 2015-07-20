using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	[SerializeField] private float m_lifeTime;

	void OnEnable () {
		Invoke("DestroyThis", m_lifeTime);
	}
	
	void OnDisable() {
		CancelInvoke("DestroyThis");
	}
	
	private void DestroyThis() {
		this.gameObject.Recycle();
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") DestroyThis();
	}
}
