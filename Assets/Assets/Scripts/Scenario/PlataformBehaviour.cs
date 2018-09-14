﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformBehaviour : MonoBehaviour {


	private AnimationCurve _oscilationCurve;

	private int _life = 5;
	private int _maxLife;
	private bool _lowered, _shaking, _defined;
	private float _loweredTime = 7.5f;
	private float _oscilationFrequency = 5;
	private float _oscilationScale = 0.01f;

	private float _offSetter;

	private void FixedUpdate() {
		if (!_defined || _shaking || _lowered)
			return;
		transform.localPosition = new Vector3(transform.localPosition.x,
			_oscilationCurve.Evaluate(Time.time * _oscilationFrequency + _offSetter) * _oscilationScale,
			transform.localPosition.z);
	}

	public void DefinePlataforms(int life, float loweredTime, float frequency, float scale, AnimationCurve curve,
		float offSet) {
		_defined = true;
		_life = life;
		_maxLife = life;
		_loweredTime = loweredTime;
		_oscilationFrequency = frequency;
		_oscilationScale = scale;
		_oscilationCurve = curve;
		_offSetter = offSet;
	}

	public void Damage() {

		if (_shaking || _lowered)
			return;

		_life--;
		if (_life <= 0) {		
			StartCoroutine(LowerAndRaise());
		}
		else {
			StartCoroutine(ShakePlataform(0.2f, 0.1f));
		}
	}

	private IEnumerator ShakePlataform(float timeToShake, float shakeIntensity) {
		_shaking = true;
		float shakeTimer = 0f;

		Vector3 lastPos = transform.localPosition;

		while (shakeTimer < timeToShake) {
			transform.localPosition += new Vector3(Random.Range(-shakeIntensity, shakeIntensity),
				Random.Range(-shakeIntensity, shakeIntensity) / 4,
				Random.Range(-shakeIntensity, shakeIntensity));
			shakeTimer += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = lastPos;
		_shaking = false;
	}

	public void Lower() {
		StopAllCoroutines();
		StartCoroutine(DoLower());
	}

	public void Raise() {
		StopAllCoroutines();
		StartCoroutine(DoRaise());
	}
	
	private IEnumerator LowerAndRaise() {
		yield return StartCoroutine(DoLower());
		yield return new WaitForSeconds(_loweredTime);
		yield return StartCoroutine(DoRaise());
	}

	private IEnumerator DoRaise() {
		while(transform.localPosition.y < _oscilationCurve.Evaluate(Time.time * _oscilationFrequency + _offSetter) * _oscilationScale) {
			transform.localPosition += Vector3.up * 3f * Time.deltaTime;
			yield return null;
		}
		transform.localPosition = new Vector3(transform.localPosition.x,
			_oscilationCurve.Evaluate(Time.time * _oscilationFrequency + _offSetter) * _oscilationScale,
			transform.localPosition.z);
		_lowered = false;
		_life = _maxLife;
	}

    private IEnumerator DoLower() {
	    _lowered = true;

	    GlobalAudio.Instance.PlayByIndex(3);
	    yield return StartCoroutine(ShakePlataform(1f, 0.2f));
		
	    while(transform.localPosition.y > -6f) {
		    transform.localPosition += Vector3.down * 3f * Time.deltaTime;
		    yield return null;
	    }

		

		
	}
}