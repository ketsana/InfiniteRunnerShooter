using UnityEngine;
using System.Collections;
using System;

public sealed class Constants {
	public const string COIN = "coin";
	public const string POWERUP_TRIPPLE_FIREBALL = "powerup_tripple_fireball";
}

public enum MinionEnemyState {
	idle = 0,
	walk,
	death,
}

public enum MovementDirection {
	up = 0,
	down,
	left,
	right,
}

[Serializable]
public class Drop {
	public GameObject objectPrefab;
	public int chance;
}
