using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurPanelBehaviour : MonoBehaviour {

	private Renderer _renderer;

	private void Awake() {
		_renderer = GetComponent<Renderer>();
		
	}

	public void Blur(float amount, Color c) {
		StopAllCoroutines();
		StartCoroutine(DoBlur(amount, c));
	}

	private IEnumerator DoBlur(float amount, Color c) {
		float lastBlurAmount = _renderer.material.GetFloat("_Size");
		Color lastColor = _renderer.material.GetColor("_Color");
		float timer = 0;
		while (timer < 0.5f) {

			_renderer.material.SetFloat("_Size", Mathf.Lerp(lastBlurAmount, amount, timer * 2f));
			_renderer.material.SetColor("_Color", Color.Lerp(lastColor, c, timer * 2));
			timer += Time.deltaTime;
			yield return null;
		}
		_renderer.material.SetFloat("_Size", amount);
		_renderer.material.SetColor("_Color", c);
	}
}
