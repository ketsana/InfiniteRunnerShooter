using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public static LevelController instance;

	public float leftLimit = -6.5f;
	public float rightLimit = 6.5f;

	[SerializeField] private Transform m_playerStartPosition;
	[SerializeField] private GameObject[] m_rooms;

	private Vector3[] m_roomDefaultPositions = new Vector3[2];

	public Vector3 PlayerStartPosition { get{ return m_playerStartPosition.position; } }

	void Awake() {
		instance = this;
	}

	void Start() {
		for(int i = 0; i < m_rooms.Length; i++) {
			m_roomDefaultPositions[i] = new Vector3();
			m_roomDefaultPositions[i] = m_rooms[i].transform.position;
		}
	}

	public float[] GetWallPositionsX() {
		return m_rooms[0].GetComponent<RoomHandler>().GetWallPositionsX();
	}

	public void RestartGame() {
		for(int i = 0; i < m_rooms.Length; i++) {
			m_rooms[i].transform.position = m_roomDefaultPositions[i];
			m_rooms[i].GetComponent<RoomHandler>().RoomInit();
		}
	}
}
