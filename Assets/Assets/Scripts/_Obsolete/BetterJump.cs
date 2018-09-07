using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour {


	[SerializeField] private float _fallMultiplier = 2.5f;

	private CustomGravity _gravity;

	private FighterBehaviour _fighter;
	
	private Rigidbody _rb;

	private ObsoletePlayerInput _input;
	
	private void Start () {
		
		_rb = GetComponent<Rigidbody>();
		_gravity = GetComponent<CustomGravity>();
		_input = GetComponentInParent<ObsoletePlayerInput>();
		_fighter = GetComponent<FighterBehaviour>();

	}

	private void FixedUpdate() {
		if (!_fighter.IsAttacking()) {
			if (_rb.velocity.y < -5.5f || (_rb.velocity.y > 0 && !_input.IsJumping())) {
				_gravity.GravityScale = _fallMultiplier;
			}
			else {
				_gravity.GravityScale = 1;
			}
		}
	}
}
