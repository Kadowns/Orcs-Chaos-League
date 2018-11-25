using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuidedMissile : MonoBehaviour {

	public float HitForce = 100;

	public float ExplosionRadius = 10;

	public float TimeToExplode;

	public float MoveSpeed = 50;
	
	private float _hitTime;

	private float _upAngle = 0;
	
	private Color _targetColor;
	
	private Transform _target;

	private Rigidbody _rb;

	private Vector3 _velocity, _acceleration;

	private MeshRenderer _renderer;

	private ScreenEffects _fx;

	private AudioSource _engine;

	private const float RotationSpeed = Mathf.Deg2Rad * 360;

	private void Awake() {
	
		_rb = GetComponent<Rigidbody>();
		_renderer = GetComponentInChildren<MeshRenderer>();
		_engine = GetComponent<AudioSource>();
		_fx = ScreenEffects.Instance;
	}

	private void Update() {
		
		if (_target != null) {
			if (_target.gameObject.activeInHierarchy) {
				_upAngle += Time.deltaTime * RotationSpeed;
				
				Vector3 up = new Vector3(Mathf.Cos(_upAngle), Mathf.Sin(_upAngle), 0.0f);
				Debug.Log(up);
				var rot = Quaternion.LookRotation((_target.position - transform.position).normalized, up);
				transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
				_velocity = (_target.position - transform.position).normalized * MoveSpeed;
				
			}
			else {
				_target = null;
			}
		}
		else {
			_velocity *= 0.98f;
			_target = FindTarget();
		}
		_engine.pitch = Mathf.Lerp(0.7f, 2.0f, _velocity.magnitude / MoveSpeed);
		
	}

	private void FixedUpdate() {
		_rb.MovePosition(transform.position + (_velocity * Time.deltaTime));
	}

	private Transform FindTarget() {
		Collider[] cols = Physics.OverlapSphere(transform.position, 900, 1<<LayerMask.NameToLayer("Players"));
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
		}
		
		
		return target;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.gameObject.layer != LayerMask.NameToLayer("Players"))
			return;

		Collider[] cols = Physics.OverlapSphere(transform.position, 15, 1 << LayerMask.NameToLayer("Players"));
		foreach (var col in cols) {
			var entity = col.GetComponent<MovableEntity>();
			var motor = entity.Motor as OrcMotor;
			var state = entity.State as OrcEntityState;
			motor.Burn(state, 150, 3f, (entity.transform.position - transform.position).normalized, 200f, -1);
		}
		_fx.ScreenShake(0.15f, 1.5f);
		_fx.FreezeFrame(0.1f);
		_fx.CreateGuidedRockExpParticles(transform.position);
		
		ResetToDefault();
		gameObject.SetActive(false);
	}

	private void ResetToDefault() {
		
	}	
}
