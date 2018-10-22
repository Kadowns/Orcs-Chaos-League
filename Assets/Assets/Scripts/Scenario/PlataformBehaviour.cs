﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformBehaviour : MonoBehaviour {

    
	[System.Serializable]
	public class PlataformSettings {

		public AnimationCurve Curve = new AnimationCurve();
		public int MaxHealth;
		public float LoweredTime;
		public float OscilationFrequency;
		public float OscilationScale;		
	}
	
	public PlataformSettings Settings { get; private set; }


	private float _offset;
	public bool Foldout { get; set; }
	private int _life { get; set; }
	private bool _lowered { get; set; }
	private bool _shaking{ get; set; }
	private bool _defined{ get; set; }

	private Material _mat;
	private float _timeToFlash = 0.1f;

	private void Awake() {
		_mat = GetComponent<Renderer>().material;
	}
	
	private void FixedUpdate() {
		if (!_defined || _shaking || _lowered)
			return;
		transform.localPosition = new Vector3(transform.localPosition.x,
			Settings.Curve.Evaluate(Time.time * Settings.OscilationFrequency + _offset) * Settings.OscilationScale,
			transform.localPosition.z);
	}

	public void DefinePlataforms(PlataformSettings settings) {
		_defined = true;
		Settings = settings;
		_offset = Random.Range(0f, 1f);
		_life = settings.MaxHealth;
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
		
		
		Vector3 lastPos = transform.localPosition;
		
		_mat.SetColor("_EmissionColor", Color.white);
		
		float shakeTimer = 0f;
		while (shakeTimer < timeToShake) {
			transform.localPosition += new Vector3(Random.Range(-shakeIntensity, shakeIntensity),
				Random.Range(-shakeIntensity, shakeIntensity) / 4,
				Random.Range(-shakeIntensity, shakeIntensity));
			
			_mat.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, shakeTimer / timeToShake));
			shakeTimer += Time.deltaTime;
			
			yield return null;
		}
		_mat.SetColor("_EmissionColor", Color.black);
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
		yield return new WaitForSeconds(Settings.LoweredTime);
		yield return StartCoroutine(DoRaise());
	}

	private IEnumerator DoRaise() {
		while(transform.localPosition.y < Settings.Curve.Evaluate(Time.time * Settings.OscilationFrequency + _offset) * Settings.OscilationScale) {
			transform.localPosition += Vector3.up * 3f * Time.deltaTime;
			yield return null;
		}
		transform.localPosition = new Vector3(transform.localPosition.x,
			Settings.Curve.Evaluate(Time.time * Settings.OscilationFrequency + _offset) * Settings.OscilationScale,
			transform.localPosition.z);
		_lowered = false;
		_life = Settings.MaxHealth;
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
