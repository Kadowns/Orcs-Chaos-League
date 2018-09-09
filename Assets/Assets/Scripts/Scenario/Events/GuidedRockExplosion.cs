using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedRockExplosion : Explosion, IEvent {
	
	public void Execute() {
		StartCoroutine(DoExplode());
	}
	
	protected override IEnumerator DoExplode() {
		_ps.Play();	
		StartCoroutine(SmoothLight(30f, 0.25f));
		_sfx.PlaySFxByIndex(0, Random.Range(0.6f, 0.9f));
		yield return new WaitForSeconds(WaitToExplode);
		_fx.ScreenShake(0.1f, 1);
		_sfx.PlaySFxByIndex(1, Random.Range(0.6f, 0.8f));
		_pool.SpawnFromPool("GuidedRock", transform.position + Vector3.up * 5, Quaternion.identity)
			.GetComponent<Rigidbody>().AddForce(Vector3.up * ExplosionForce);
			
		_ps.Stop();
		StartCoroutine(SmoothLight(0f, 0.75f));
	}
}
