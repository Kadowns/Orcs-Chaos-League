using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaRockBehaviour : MonoBehaviour, IThrowable {

	public float HitForce = 100;

	public float HitDelayTime = 1.5f;

	public float InitialSpeed = 60f;

	private float _accumulatedSpeed = 1;
	
	private bool _wasHit;

	private float _hitTime;
	
	private Transform _target;

	private Rigidbody _rb;
	
	private Vector3 _velocity, _acceleration;

	private void Awake() {
	
		_rb = GetComponent<Rigidbody>();
	}

	private void Update () {
		if (_wasHit && _hitTime < Time.time) {
			_wasHit = false;
		}
		else {
			if (_target != null) {
				_velocity = (_target.position - transform.position).normalized * InitialSpeed * _accumulatedSpeed;
				_velocity += Vector3.up * (_target.position.y + 3.5f + transform.localScale.y / 2 - transform.position.y);
			}
			else {
				_target = FindTarget();
			}			
		}
	}

	private void FixedUpdate() {
		if (!_wasHit) {
			_rb.MovePosition(transform.position + (_velocity * Time.deltaTime));
		}
	}

	private Transform FindTarget() {
		Collider[] cols = Physics.OverlapSphere(transform.position, 90, 1<<11);
		Transform target = null;
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			
			float dist = (col.transform.position - transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				target = col.transform;
			}
		}
		return target;
	}

	public void Throw(Vector3 dir) {
		_accumulatedSpeed += 0.5f;
		transform.localScale += Vector3.one;
		_wasHit = true;
		var latDir = new Vector3(dir.x, 0, dir.z);
		_rb.AddForce(latDir * HitForce, ForceMode.Impulse);
		_rb.AddTorque(new Vector3(latDir.z, 0, latDir.x) * HitForce, ForceMode.Impulse);
		_hitTime = Time.time + HitDelayTime;
		
	}
}
