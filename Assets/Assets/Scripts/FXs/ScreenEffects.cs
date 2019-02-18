using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffects : Singleton<ScreenEffects> {


	private ObjectPooler _pool;

	private Transform _cameraPivot, _camera;

	private Coroutine _screenShakeRoutine;
	private Coroutine _blurForSecondsRoutine;
	
	private Quaternion _lastRotation;
	
	private float _shakeIntensity;
	
	private bool  _freezeFrame;

	[SerializeField] private BlurPanelBehaviour _blurPanel;

	private void Awake() {
		_cameraPivot = CameraController.Instance.transform;
		_camera = Camera.main.transform;
		_pool = ObjectPooler.Instance;
	}

	public void Blur(float amount, Color c) {
		_blurPanel.Blur(amount, c);
	}

	public void BlurForSeconds(float seconds, float intensity, Color startColor, Color endColor) {
		if (_blurForSecondsRoutine != null)
			StopCoroutine(_blurForSecondsRoutine);

		_blurForSecondsRoutine = StartCoroutine(DoBlurForSeconds(seconds, intensity, startColor, endColor));

	}
	
	private IEnumerator DoBlurForSeconds(float seconds, float intensity, Color startColor, Color endColor) {
		
		Blur(intensity, startColor);
		yield return new WaitForSeconds(seconds);
		Blur(0f, endColor);
		
	}

	public void CreateShield(Transform parent) {
		GameObject obj = _pool.SpawnFromPool("Shield", parent.position, Quaternion.identity, parent);		
		obj.GetComponent<DestroyByDuration>().InactiveAfterSeconds(4f);
	}

	public void CreateBloodSplashParticles(Vector3 position) {
		GameObject obj = _pool.SpawnFromPool("BloodSplashPS", position, Quaternion.identity);
		obj.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	public void CreatePlusOneParticles(Vector3 position) {
		GameObject obj = _pool.SpawnFromPool("PlusOnePS", position, Quaternion.identity);
		obj.GetComponent<DestroyByDuration>().InactiveAfterSeconds(1.5f);
	}

	public void CreateMeteoriteParticles(Vector3 position) {
		GameObject obj = _pool.SpawnFromPool("MeteoritePS", position, Quaternion.identity);
		obj.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	public void CreateDeadOrc(Vector3 position) {
		GameObject deadOrc = _pool.SpawnFromPool("DeadOrc", position, Quaternion.identity);
		deadOrc.GetComponent<DestroyByDuration>().InactiveAfterSeconds(1.5f);
	}
	
	public void CreateCageParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("CagePS", position, Quaternion.identity);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}
	
	public void CreateGuidedMissileExpParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("GuidedRockExpPS", position, Quaternion.identity);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	public void CreateRockExpParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("RockExpPS", position, Quaternion.identity);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	public void CreateSmokeParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("SmokePS", position, Quaternion.identity);
		ps.GetComponent<DestroyByDuration>().InactiveAfterSeconds(2f);
	}
	
	public void CreateBoxParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("BoxPS", position, Quaternion.identity);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	public void CreateDashParticles(Transform parent, Vector3 dir) {
		GameObject ps = _pool.SpawnFromPool("DashPS", parent.position + Vector3.up * 2,  Quaternion.LookRotation(dir), parent);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}
	
	public void CreateDashParticles(Vector3 position, Transform parent, Vector3 dir) {
		GameObject ps = _pool.SpawnFromPool("DashPS", position,  Quaternion.LookRotation(dir), parent);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	//Particulas de stun
	public void CreateHitParticles(Transform parent, Vector3 dir) {		
		GameObject ps = _pool.SpawnFromPool("HitPS", parent.position +  (Vector3.up * 2),  Quaternion.LookRotation(dir), parent);
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}
	
	//Particulas de fogo
	public void CreateBurningParticles(Transform parent, float duration) {
		var ps = _pool.SpawnFromPool("FirePS", parent.position, Quaternion.identity, parent).GetComponent<ParticleSystem>();
	    ps.Stop();	
		var main = ps.main;
		main.duration = duration;
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
		ps.Play();
	}
	public void CreateBurningParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("FirePS", position, Quaternion.identity); 
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	//Particulas de stomp
	public void CreateStompParticles(Vector3 position) {
		GameObject ps = _pool.SpawnFromPool("StompPS", position, Quaternion.identity); 
		ps.GetComponent<DestroyByDuration>().InactiveAfterFineshed();
	}

	//Freeze frame (avá)
	public void FreezeFrame(float recoverySpeed) {
		Time.timeScale = 0;
		TimeController.Instance.DoTimeScale(1, recoverySpeed);
	}
	
	//ScreenShake (avá2)
	public void ScreenShake(float timeToShake, float intensity) {

		if (CameraController.Instance.InTransition())
			return;
		
		if (_screenShakeRoutine != null) {
			StopCoroutine(_screenShakeRoutine);
			_camera.rotation = _lastRotation;
		}		
		
		_screenShakeRoutine = StartCoroutine(DoScreenShake(timeToShake, intensity));
	}

	private IEnumerator DoScreenShake(float timeToShake, float intensity) {	
		CameraController.Instance.ExternAgent(true);
		_lastRotation = _camera.rotation;
		float shakeTimer = 0;
		while (shakeTimer < timeToShake) {
			var random = Random.insideUnitSphere * intensity;
			_cameraPivot.position += random;
			_camera.rotation = Quaternion.Euler(_camera.rotation.eulerAngles.x, _camera.rotation.eulerAngles.y,
				Mathf.Sin(shakeTimer * Mathf.PI * 16) * intensity * 5);
			intensity *= 0.9f;
			shakeTimer += Time.deltaTime;
			yield return null;
		}
		CameraController.Instance.ExternAgent(false);
		shakeTimer = 0;
		while (shakeTimer < 0.25f) {
			_camera.rotation = Quaternion.Lerp(_camera.rotation, _lastRotation, shakeTimer / 0.25f);
			shakeTimer += Time.deltaTime;
			yield return null;
		}
		_camera.rotation = _lastRotation;
	}
}
