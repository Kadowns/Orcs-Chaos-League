using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoBombBehaviour : MonoBehaviour, ISpawnable {

	public float TimeToExplode;
	public float MoveSpeed = 100f;
	public float YOffset = 10;

	[SerializeField] private float _spawnForce;

	private Rigidbody _rb;
	
	private Transform _target;

	private float _timer = 0;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		if (_target == null || !_target.gameObject.activeInHierarchy) {
			FindClosestOrc();
		}
		else {
			var dir = ((_target.transform.position + Vector3.up * YOffset) - transform.position) * 2f;
			_rb.MovePosition(transform.position + (dir * MoveSpeed * Time.deltaTime));
		}
	}

	public void OnSpawn() {
		_rb.AddForce(Vector3.up * _spawnForce, ForceMode.Impulse);
	}

	public void FindClosestOrc() {
		var cols = Physics.OverlapSphere(transform.position, 500f, 1 << 11);
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			float dist = (col.transform.position - transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				_target = col.transform;
			}
		}
	}
}
