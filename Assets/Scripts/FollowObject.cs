using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public Transform targetToFollow;
	public float distanceToTarget;
	
	void Start () {
		if(targetToFollow == null && GameObject.Find("Player")) {
			targetToFollow = GameObject.Find("Player").transform;
		}

		if(distanceToTarget == 0) {
			if(targetToFollow != null) {
				distanceToTarget = targetToFollow.position.z - this.transform.position.z;
			}
		}
	}
	

	void LateUpdate () {
		if(targetToFollow != null && distanceToTarget != 0) {
			Vector3 newPosition = this.transform.position;
			newPosition.z = targetToFollow.position.z - distanceToTarget;
			this.transform.position = newPosition;
		}
	}
}
