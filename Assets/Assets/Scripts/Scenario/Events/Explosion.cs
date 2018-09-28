using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IEvent {

	public float ExplosionForce;
	public float WaitToExplode;
	public int ObjectsToSpawn;
	public string ObjectName;

	protected Light _light;	
	protected SoundEffectPlayer _sfx;
	protected ParticleSystem _ps;	
	protected ObjectPooler _pool;
	protected ScreenEffects _fx;

	private void Start () {
		_ps = GetComponent<ParticleSystem>();
		_sfx = GetComponent<SoundEffectPlayer>();
		_light = GetComponent<Light>();
		_light.intensity = 0;
		_pool = ObjectPooler.Instance;
		_fx = ScreenEffects.Instance;
	}
	
	protected IEnumerator SmoothLight(float targetIntensity, float timeToSmooth) {
		float lastIntensity = _light.intensity;
		float timer = 0;
		while (timer < timeToSmooth) {
			_light.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / timeToSmooth);
			timer += Time.deltaTime;
			yield return null;
		}

		_light.intensity = targetIntensity;
	}
	
	public void Execute() {
		StartCoroutine(DoExplode());
	}

	protected IEnumerator DoExplode() {
		_ps.Play();	
		StartCoroutine(SmoothLight(30f, 0.25f));
		_sfx.PlaySFxByIndex(0, Random.Range(0.6f, 0.9f));
		yield return new WaitForSeconds(WaitToExplode);
		_fx.ScreenShake(0.1f, 1);
		_sfx.PlaySFxByIndex(1, Random.Range(0.6f, 0.8f));
		for (int i = 0; i < ObjectsToSpawn; i++) {
			var rock = _pool.SpawnFromPool(ObjectName, transform.position + Vector3.up * 2, Quaternion.identity);

			var dir = (-transform.position.normalized +
			           new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f)).normalized).normalized;
			rock.GetComponent<Rigidbody>().AddForce(dir * ExplosionForce / Random.Range(1, 3), ForceMode.Impulse);
			yield return null;
		}
		_ps.Stop();
		StartCoroutine(SmoothLight(0f, 0.75f));
	}
}
