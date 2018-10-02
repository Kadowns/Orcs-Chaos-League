using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHeadItem : CollectableItem {

	public float MoveSpeed = 100f;
	public float SpawnForce = 100f;

	private Rigidbody _rb;
	private Transform _target;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
		
		var magnet = GetComponentInChildren<MagnetCollider>();
		if (magnet != null) {
			magnet.DetectedColliderEvent += DetectedCollider;
			
		}
	}

	private void DetectedCollider(Collider other) {
		if (other.gameObject.layer != LayerMask.NameToLayer("Players") || _target != null)
			return;
		
		_rb.isKinematic = true;
		_target = other.transform;
	}
	
	private void FixedUpdate() {
		if (_target != null) {
			if (_target.gameObject.activeInHierarchy) {
				_rb.MovePosition(Vector3.Lerp(_rb.position, _target.position, MoveSpeed * Time.deltaTime));
				
			}
			else {
				_rb.isKinematic = false;
				_target = null;
				
			}
		}	
	}

	private void OnCollisionEnter(Collision other) {
		if (LayerMask.NameToLayer("Lava") == other.gameObject.layer && _target == null) {
			_rb.AddForce((-_rb.position.normalized + Vector3.up).normalized * SpawnForce , ForceMode.Impulse);
			
		} else if (LayerMask.NameToLayer("Players") == other.gameObject.layer) {		
			var otherEntity = other.gameObject.GetComponent<MovableEntity>();
			var otherMotor = otherEntity.Motor as OrcMotor;
			var otherState = otherEntity.State as OrcEntityState;
			otherMotor.CollectItem(otherState, Type);
			Collect();	
		}
	}

	public override void Collect() {
		gameObject.SetActive(false);
	}

	public override void OnSpawn() {
		_rb.isKinematic = false;
		
		_rb.MovePosition(transform.position + Vector3.up);
			
		_rb.AddForce(
			(Vector3.up  * Random.Range(2, 6)+
			 Vector3.right * Random.Range(-1, 1) +
			 Vector3.forward * Random.Range(-1, 1) - transform.position.normalized).normalized * SpawnForce,
			ForceMode.Impulse);

	}
}
