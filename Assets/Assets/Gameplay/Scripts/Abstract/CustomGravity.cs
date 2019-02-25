using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour {

	public bool Gravity;
	public float GravityScale = 1;
	
	protected Rigidbody _rb;
	
	private void Awake() {
		_rb = GetComponent<Rigidbody>();
		_rb.useGravity = false;
	}

	private void FixedUpdate() {
		if (Gravity)
			_rb.AddForce(Physics.gravity * GravityScale, ForceMode.Acceleration);
	}
}
