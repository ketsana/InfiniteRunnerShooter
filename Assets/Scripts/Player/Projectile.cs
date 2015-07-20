using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[SerializeField] private float m_lifeTime;
	[SerializeField] private float m_forwardSpeed;
	[SerializeField] private float m_sideSpeed;
	[SerializeField] private MovementDirection m_direction;

	void OnEnable () {
		Invoke("DestroyThis", m_lifeTime);
	}

	void OnDisable() {
		CancelInvoke("DestroyThis");
	}

	private void DestroyThis() {
		this.gameObject.Recycle();
	}

	void Update () {
		// forward movement
		transform.Translate(Vector3.forward * m_forwardSpeed * Time.deltaTime);

		// side movement
		if(m_direction == MovementDirection.left) transform.Translate(Vector3.left * m_sideSpeed * Time.deltaTime);
		else if(m_direction == MovementDirection.right) transform.Translate(Vector3.right * m_sideSpeed * Time.deltaTime);
	}

	void OnTriggerEnter (Collider collider) {
//		Debug.Log (collider.tag);
		if (collider.tag == "Obstacle" || collider.tag == "MinionEnemy") DestroyThis();
//		else if (collider.tag == "MinionEnemy") DestroyThis();
	}
}
