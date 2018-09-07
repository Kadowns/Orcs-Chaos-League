using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCable : MonoBehaviour {

	[SerializeField] private bool _shouldApplyForce;
	[SerializeField] private float _cableLength;
	[SerializeField] private float _cableStrength;
	[SerializeField] private Transform _endPoint;
	
	private LineRenderer _line;
	private Rigidbody _endRb;
	
	private void Start() {
		_line = GetComponent<LineRenderer>();
		_line.positionCount = 2;
		_line.SetPosition(0, transform.position);
		_line.SetPosition(1, _endPoint.position);
		if (_shouldApplyForce)
			_endRb = _endPoint.GetComponent<Rigidbody>();
	}

	private void Update() {
		if (!_shouldApplyForce)
			return;
		
		float dist = (_endPoint.position - transform.position).sqrMagnitude;

		if (dist > _cableLength * _cableLength) {

			Vector3 ropeTension = transform.position - _endPoint.position ;

			_endRb.AddForce(ropeTension * _cableStrength * Time.deltaTime, ForceMode.Force);

		}
	}

	private void LateUpdate() {
		_line.SetPosition(0, transform.position);
		_line.SetPosition(1, _endPoint.position);
	}
}
