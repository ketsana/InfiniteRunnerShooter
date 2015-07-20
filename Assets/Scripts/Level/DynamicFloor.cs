using UnityEngine;
using System.Collections;

public class DynamicFloor : MonoBehaviour {

	private RoomHandler roomHandler;

	void Start() {
		roomHandler = this.GetComponent<RoomHandler>();
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Player") {
			Vector3 _newPos = this.transform.position;
			this.transform.position = new Vector3(_newPos.x, _newPos.y, _newPos.z + 100);
			roomHandler.RoomInit();
		}
	}
}
