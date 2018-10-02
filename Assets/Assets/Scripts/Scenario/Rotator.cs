using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] private float _rotationFrequency;

	[SerializeField] private float _rotationSpeed;

	[SerializeField] private AnimationCurve _rotationCurve;

    private float _trueRotationSpeed;

	private void FixedUpdate() {
		_trueRotationSpeed = _rotationCurve.Evaluate(Time.time / _rotationFrequency) * _rotationSpeed;
		transform.Rotate(new Vector3(0, _trueRotationSpeed, 0) * Time.deltaTime);
	}
}
