using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

	[SerializeField] private float _frequency = 5;
	[SerializeField] private float _scale = 0.01f;

	private float _offSetter;
	// Use this for initialization
	void Start () {
		_offSetter = Random.Range(-10, 10);
	}

	private void FixedUpdate() {

		transform.position += Vector3.up * (Mathf.Sin(Time.time + _offSetter) * _frequency) * _scale;
	}
}
