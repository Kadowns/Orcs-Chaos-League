using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHeadItem : CollectableItem {

	public float MoveSpeed = 100f;
	
	private Transform _target;
	
	private void FixedUpdate() {

		if (_target != null) {
			var dir = (_target.position - transform.position).normalized;
			_rb.AddForce(dir * MoveSpeed, ForceMode.Acceleration);
		}
		else {
			_rb.velocity *= 0.97f;
			FindClosestOrc();
		}
	}

	public override void OnSpawn() {
		CanBeCollected = false;
		WillBeAbleToCollect = Time.time + TimeDelayToCollect;
		_rb.MovePosition(transform.position + Vector3.up * 8);
		
		
		_rb.AddForce(
			(Vector3.up  * Random.Range(2, 6)+
			 Vector3.right * Random.Range(-1, 1) +
			 Vector3.forward * Random.Range(-1, 1) - transform.position.normalized).normalized * SpawnForce,
			ForceMode.Impulse);
		
		FindClosestOrc();
	}

	private void FindClosestOrc() {
		var orcs = Physics.OverlapSphere(transform.position, 200, 1 << 11);
		float minDist = float.MaxValue;
		foreach (var orc in orcs) {
			float dist = (orc.transform.position - transform.position).sqrMagnitude;
			if (dist < minDist) {
				_target = orc.transform;
				minDist = dist;
			}
		}
	}
}
