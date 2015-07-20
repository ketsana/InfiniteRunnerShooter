using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WallBlock {
	public Transform start;
	public Transform end;
	public int noOfWalls;
	public int maxColumn;
	public int maxRow;
}

[System.Serializable]
public class EnemyBlock {
	public Transform start;
	public Transform end;
	public int noOfEnemies;
	public int maxColumn;
	public int maxRow;
}

public class RoomHandler : MonoBehaviour {

	public List<GameObject> usedObstacles = new List<GameObject>();

	public MinionEnemy[,] minionEnemies = new MinionEnemy[7,13];
	public List<GameObject> usedMinionEnemies = new List<GameObject>();

	[SerializeField] private WallBlock[] m_wallBlocks = default(WallBlock[]);
	[SerializeField] private GameObject[] m_wallPrefabs = default(GameObject[]);
	[SerializeField] private float distancePerWallX = default(float);
	[SerializeField] private float[] wallPositionsY = default(float[]);
	[SerializeField] private float distancePerWallZ = default(float);

	[SerializeField] private EnemyBlock[] m_enemyBlocks = default(EnemyBlock[]);
	[SerializeField] private GameObject[] m_enemyPrefabs = default(GameObject[]);
	[SerializeField] private float distancePerEnemyX = default(float);
	[SerializeField] private float enemyPositionY = default(float);
	[SerializeField] private float distancePerEnemyZ = default(float);

	void Awake() {
		Init();
	}

	private void Init() {
		foreach(WallBlock wb in m_wallBlocks) {
			int _maxRows = Mathf.CeilToInt((wb.end.position.z - wb.start.position.z) / distancePerWallZ);
			if(wb.maxRow > _maxRows) wb.maxRow = _maxRows;

			wb.start.gameObject.SetActive(false);
			wb.end.gameObject.SetActive(false);
		}

		foreach(EnemyBlock eb in m_enemyBlocks) {
			int _maxRows = Mathf.CeilToInt((eb.end.position.z - eb.start.position.z) / distancePerWallZ);
			if(eb.maxRow > _maxRows) eb.maxRow = _maxRows;
			
			eb.start.gameObject.SetActive(false);
			eb.end.gameObject.SetActive(false);
		}
	}

	void Start() {
		RoomInit();
	}

	public void RoomInit() {
		SetupWalls();
		SetupMinions();
	}

	public float[] GetWallPositionsX() {
		float[] _wallPositionsX = new float[m_wallBlocks[0].maxColumn];
		for(int i = 0; i < m_wallBlocks[0].maxColumn; i++) {
			_wallPositionsX[i] = m_wallBlocks[0].start.position.x + (i * distancePerWallX);
		}

		return _wallPositionsX;
	}

	private void SetupWalls() {
		foreach(GameObject go in usedObstacles) {
			if(go) {
				if(go.activeSelf) go.Recycle();
			}
		}
		usedObstacles.Clear();

		foreach(WallBlock wb in m_wallBlocks) {
			for(int i = 0; i < wb.noOfWalls; i++) {
				int _randomColumn = Random.Range(0, wb.maxColumn);
				int _randomRow = Random.Range(0, wb.maxRow);
				float _x = wb.start.position.x + (_randomColumn * distancePerWallX);
				float _y = wallPositionsY[Random.Range(0, wallPositionsY.Length)];
				float _z = wb.start.position.z + (_randomRow * distancePerWallZ);
				Vector3 _pos = new Vector3(_x, _y, _z);
				GameObject _obj = m_wallPrefabs[Random.Range(0, m_wallPrefabs.Length)].Spawn(_pos, Quaternion.identity);
				usedObstacles.Add(_obj);
			}
		}
	}

	private void SetupMinions() {
		foreach(GameObject go in usedMinionEnemies) {
			if(go.activeSelf) go.Recycle();
		}
		usedMinionEnemies.Clear();
		
		foreach(EnemyBlock eb in m_enemyBlocks) {
			for(int i = 0; i < eb.noOfEnemies; i++) {
				int _randomColumn = Random.Range(0, eb.maxColumn);
				int _randomRow = Random.Range(0, eb.maxRow);
				float _x = eb.start.position.x + (_randomColumn * distancePerEnemyX);
				float _y = enemyPositionY;
				float _z = eb.start.position.z + (_randomRow * distancePerEnemyZ);
				Vector3 _pos = new Vector3(_x, _y, _z);
				GameObject _obj = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)].Spawn(_pos, Quaternion.identity);
				usedMinionEnemies.Add(_obj);
			}
		}
	}
}
