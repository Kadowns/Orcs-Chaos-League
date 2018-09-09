using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosion : MonoBehaviour {

	public float ExplosionForce;
	public float WaitToExplode;

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

	protected abstract IEnumerator DoExplode();
	
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
}
