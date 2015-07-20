using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Slider slider;
	public SpriteColorLerp healthColor;
	public SpriteColorLerp healthBarColor;

	public float currentHp;
	public float maxHp;

	public void UpdateHealthBar(float p_currentHp, float p_maxHp) {
		currentHp = p_currentHp;
		maxHp = p_maxHp;
		float _value = p_currentHp / p_maxHp;
		_value = Mathf.Clamp(_value, 0f, 1f);
		slider.value = _value;
		healthColor.cValue = _value;
		healthBarColor.cValue = _value;
	}
}
