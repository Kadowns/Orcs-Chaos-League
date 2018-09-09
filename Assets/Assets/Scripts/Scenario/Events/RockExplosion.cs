using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockExplosion : Explosion, IEvent {

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
		for (int i = 0; i < 5; i++) {	
			var rock = _pool.SpawnFromPool("LavaRock", transform.position + Vector3.up * 2, Quaternion.identity);

			var dir = (-transform.position.normalized +
			           new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f)).normalized).normalized;
			rock.GetComponent<Rigidbody>().AddForce(dir * ExplosionForce / Random.Range(1, 3), ForceMode.Impulse);
			yield return null;
		}
		_ps.Stop();
		StartCoroutine(SmoothLight(0f, 0.75f));
	}

}
