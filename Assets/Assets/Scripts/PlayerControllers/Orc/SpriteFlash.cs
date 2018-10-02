using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SpriteFlash : MonoBehaviour {

	private Renderer _renderer;
	private Coroutine _whiteFlashRoutine;
	private Coroutine _redFlashRoutine;

	public void Awake() {
		_renderer = GetComponentInChildren<Renderer>();
	}

	public void SetRedAmount(float amount) {
		var redAmount = amount / 100;
		
		if (_redFlashRoutine != null) 
			StopCoroutine(_redFlashRoutine);
		
		_redFlashRoutine = StartCoroutine(DamageFlashing(Mathf.Lerp(1, 10, redAmount), redAmount > 0.3f ? 0.3f : redAmount));
	}

	private IEnumerator DamageFlashing(float frequency, float amount) {
		while (true) {
			float whiteValue =  Mathf.Abs(Mathf.Sin(Time.time * frequency) * amount);
			_renderer.material.SetFloat("_FlashAmount", whiteValue);
			yield return null;
		}
	}

	public void ResetToDefault() {
		StopAllCoroutines();
		_renderer.material.SetColor("_Color", Color.white);
		_renderer.material.SetFloat("_FlashAmount", 0f);
	}
	
	
	public void ImpactFlashAfterSeconds(Color flashColor, float waitTime, float timeToFlash, float intensity) {
		_renderer.material.SetColor("_FlashColor", flashColor);
		
		if (_whiteFlashRoutine != null)
			StopCoroutine(_whiteFlashRoutine);
		
		_whiteFlashRoutine = StartCoroutine(FlashHitColor(timeToFlash, waitTime, intensity));
	}

	public void ImpactFlash(Color flashColor, float timeToFlash, float intensity) {
		if (!gameObject.activeInHierarchy)
			return;
		_renderer.material.SetColor("_FlashColor", flashColor);
		
		if (_whiteFlashRoutine != null)
			StopCoroutine(_whiteFlashRoutine);
		
		_whiteFlashRoutine = StartCoroutine(FlashHitColor(timeToFlash, 0, intensity));
	}

	public void SetTintColor(Color c) {
		_renderer.material.SetColor("_Color", c);
	}

	private IEnumerator FlashHitColor(float timeToLerp, float waitTime,float flashIntensity) {
		_renderer.material.SetFloat("_FlashAmount", flashIntensity);
		yield return new WaitForSeconds(waitTime);
		float timer = 0;
		while (timer < timeToLerp) {
			timer += Time.deltaTime;
			_renderer.material.SetFloat("_FlashAmount", Mathf.Lerp(flashIntensity, 0f, timer / timeToLerp));
			yield return null;
		}

		_renderer.material.SetFloat("_FlashAmount", 0f);
	}
	
}
