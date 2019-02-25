using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuidedMissile : MonoBehaviour, ISpawnable {

	public float HitForce = 100;

	public float ExplosionRadius = 10;

	public float TimeToExplode;

	public float MoveSpeed = 50;
	
	private float _hitTime;

	private float _upAngle = 0;
	
	private Color _targetColor;
	
	private Transform _target;

	private Rigidbody _rb;

	private Vector3 _velocity;

	private MeshRenderer _renderer;

	private TrailRenderer _trail;

	private ScreenEffects _fx;

	private SoundEffectPlayer _sfx;

	private Coroutine _pursuitRoutine;

	private const float RotationSpeed = Mathf.Deg2Rad * 360;

	private void Awake() {
	
		_rb = GetComponent<Rigidbody>();
		_renderer = GetComponentInChildren<MeshRenderer>();
		_trail = GetComponentInChildren<TrailRenderer>();
		_sfx = GetComponent<SoundEffectPlayer>();
		_fx = ScreenEffects.Instance;
	}

	private void Start() {
		var sources = GetComponents<AudioSource>();
		foreach (var source in sources) {
			source.priority = 255;
		}
	}

	private void Update() {
		if (_pursuitRoutine == null && gameObject.activeInHierarchy) {
			_pursuitRoutine = StartCoroutine(PursuitAndExplode());
		}
		
		
	}

	private Transform FindTarget() {
		Collider[] cols = Physics.OverlapSphere(transform.position, 900, 1 << LayerMask.NameToLayer("Players"));
		Transform target = null;
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			float dist = (col.transform.position - transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				target = col.transform;
			}
		}

		if (target != null) {
			_targetColor = target.GetComponent<OrcEntityState>().PlayerColor;
			_renderer.material.SetColor("_EmissionColor", _targetColor * 5);
			_trail.startColor = Color.white;
			_trail.endColor = _targetColor;
		}
		else {
			_trail.startColor = Color.white;
		}
		
		
		return target;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.gameObject.layer != LayerMask.NameToLayer("Players"))
			return;

		Detonate();
	}

	private void Detonate() {
		Collider[] cols = Physics.OverlapSphere(transform.position, ExplosionRadius, 1 << LayerMask.NameToLayer("Players"));
		foreach (var col in cols) {
			var entity = col.GetComponent<MovableEntity>();
			var motor = entity.Motor as OrcMotor;
			var state = entity.State as OrcEntityState;
			motor.Burn(state, (int)HitForce, 3f, (entity.transform.position - transform.position).normalized, HitForce, -1);
		}
		_fx.ScreenShake(0.15f, 1.5f);
		_fx.FreezeFrame(0.1f);
		_fx.CreateGuidedMissileExpParticles(transform.position);
		
		ResetToDefault();
		gameObject.SetActive(false);
	}

	private void ResetToDefault() {
		if (_pursuitRoutine != null) {
			StopCoroutine(_pursuitRoutine);
		}
	}

	public void OnSpawn() {

		_pursuitRoutine = StartCoroutine(PursuitAndExplode());
	}

	private IEnumerator PursuitAndExplode() {
		float timer = 0;
		float beepTimer = 0;
		while (timer < TimeToExplode) {
			timer += Time.deltaTime;
			beepTimer += Time.deltaTime;
			Vector3 targetPosition = transform.forward;
			
			if (_target != null) {
				if (_target.gameObject.activeInHierarchy) {
					targetPosition = _target.position;
				}
				else {				
					_target = null;				
				}
			}
			else {
				_target = FindTarget();
				float t = Time.time * 2;
				targetPosition = new Vector3(Mathf.Cos(t) * 20, 20f, Mathf.Sin(t) * 20);
				
			}

			if (0.2f < (beepTimer / TimeToExplode)) {
				_sfx.PlaySFxByIndex(0, 1f);
				beepTimer = 0;
			}
			
			_upAngle += Time.deltaTime * RotationSpeed;
			var dir = (targetPosition - transform.position).normalized;
			var up = new Vector3(Mathf.Cos(_upAngle), Mathf.Sin(_upAngle), 0.0f);
			var rot = Quaternion.LookRotation(dir, up);
			_rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, 0.1f));
			
			_velocity = transform.forward * (MoveSpeed * (timer + 1)) * Time.deltaTime;
			_rb.MovePosition(transform.position + _velocity);
			yield return null;
		}

		beepTimer = 0;
		timer = 0;
		while (timer < 1f) {
			timer += Time.deltaTime;
			beepTimer += Time.deltaTime;
			
			if (0.1f < (beepTimer)) {
				_sfx.PlaySFxByIndex(0, 1.5f);
				beepTimer = 0;
			}
			
			_velocity = transform.forward * (MoveSpeed * TimeToExplode * 2) * Time.deltaTime;
			_rb.MovePosition(transform.position + _velocity);
			
			yield return null;
		}
		
		Detonate();
	}
}
