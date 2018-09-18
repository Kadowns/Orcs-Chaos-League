using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHeadItem : CollectableItem {
	public override void OnSpawn() {
		CanBeCollected = false;
		WillBeAbleToCollect = Time.time + TimeDelayToCollect;
		_rb.MovePosition(transform.position + Vector3.up * 8);
		
		
		_rb.AddForce(
			(Vector3.up  * Random.Range(2, 6)+
			 Vector3.right * Random.Range(-1, 1) +
			 Vector3.forward * Random.Range(-1, 1) - transform.position.normalized).normalized * SpawnForce,
			ForceMode.Impulse);
	}
}
