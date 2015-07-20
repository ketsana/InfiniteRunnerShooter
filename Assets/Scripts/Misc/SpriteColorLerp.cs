using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Color Lerp for the new UI Image sprite.
 * value is clamp to 0.0 - 1.0
 * - Ketsana
 * 
 */

public class SpriteColorLerp : MonoBehaviour {

	public Image sprite;
	public Color colorMinValue;
	public Color colorMaxValue;
	public float cValue;

	private float m_value {
		get {
			return Mathf.Clamp(cValue, 0, 1f);
		}
		set {
			value = cValue;
		}
	}

	void Awake() {
		if(!sprite) sprite = this.GetComponent<Image>();
	}

	void Update() {
		m_value = cValue;
		sprite.color = Color.Lerp(colorMinValue, colorMaxValue, m_value);
	}
}
