using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuidedRockBehaviour : MonoBehaviour, IThrowable {

	public float HitForce = 100;

	public float HitDelayTime = 1.5f;

	public int MaximumHits = 3;

	public float InitialSpeed = 60f;

	public float ExplosionDelayTime = 2f;

	public float ScaleFactor = 2;
	
	public int AttackerId { get; set; }

	public AnimationCurve HitScaleCurve;
	public AnimationCurve ExplosionScaleCurve;

	private int _numberOfHits;
	
	private bool _wasHit, _exploding;

	private float _hitTime;

	private Color _targetColor;
	
	private Transform _target;

	private Rigidbody _rb;

	private SoundEffectPlayer _sfx;
	
	private Vector3 _velocity, _acceleration;

	private MeshRenderer _renderer;

	private Light _light;

	private ScreenEffects _fx;

	private void Awake() {
	
		_rb = GetComponent<Rigidbody>();
		_sfx = GetComponent<SoundEffectPlayer>();
		_renderer = GetComponentInChildren<MeshRenderer>();
		_light = GetComponent<Light>();
		_fx = ScreenEffects.Instance;
	}

	private void Update() {
		if (_wasHit || _exploding) {
			return;
		}

		if (_target != null && _target.gameObject.activeInHierarchy) {
			_velocity = (_target.position - transform.position).normalized * InitialSpeed * (_numberOfHits + 1);
			_velocity += Vector3.up * (_target.position.y + 3.5f + transform.localScale.y / 2 - transform.position.y);
		}
		else {
			_target = FindTarget();
		}
	}

	private void FixedUpdate() {
		if (_wasHit || _exploding)
			return;
		_rb.MovePosition(transform.position + (_velocity * Time.deltaTime));
	}

	private Transform FindTarget() {
		Collider[] cols = Physics.OverlapSphere(transform.position, 900, 1<<11);
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

	public void Throw(Vector3 dir, int attackerID) {
		if (_wasHit)
			return;

		_numberOfHits++;
		AttackerId = attackerID;

		var latDir = new Vector3(dir.x, 0, dir.z);
		_rb.AddForce(latDir * HitForce, ForceMode.Impulse);
		_rb.AddTorque(new Vector3(latDir.z, 0, latDir.x) * HitForce, ForceMode.Impulse);

		if (_numberOfHits >= MaximumHits) {
			StartCoroutine(DoExplosion());
		}
		else {
			StartCoroutine(WasHit());
		}		
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.gameObject.layer != 11)
			return;

		var entity = other.collider.GetComponent<MovableEntity>();
		var motor = entity.Motor as OrcMotor;
		var state = entity.State as OrcEntityState;
		var dir = (entity.transform.position - transform.position).normalized;
		if (!state.Parrying) {
			motor.Burn(state, 80, 0.5f, dir, 150, AttackerId);
			StartCoroutine(DoExplosion());
		}
		else {
			motor.DoCounter(state);
			Throw(-dir, AttackerId);
		}
	}

	private IEnumerator DoExplosion() {
		_exploding = true;
		Vector3 lastScale = transform.localScale;
		float timer = 0;
		while (timer < ExplosionDelayTime) {
			var curveValue = ExplosionScaleCurve.Evaluate(timer / ExplosionDelayTime);
			transform.localScale = Vector3.Lerp(lastScale, Vector3.one * curveValue, timer / ExplosionDelayTime);
			_renderer.material.SetColor("_EmissionColor", _targetColor * Mathf.Lerp(15, 5, curveValue * curveValue));
			_light.intensity = curveValue * curveValue * 1.2f;
			timer += Time.deltaTime;
			yield return null;
		}
		Collider[] cols = Physics.OverlapSphere(transform.position, 15, 1<<11);
		foreach (var col in cols) {
			var entity = col.GetComponent<MovableEntity>();
			var motor = entity.Motor as OrcMotor;
			var state = entity.State as OrcEntityState;
			motor.Burn(state, 150, 3f, (entity.transform.position - transform.position).normalized, 200f, AttackerId);
		}
		_fx.ScreenShake(0.15f, 1.5f);
		_fx.FreezeFrame(0.1f);
		_fx.CreateGuidedRockExpParticles(transform.position);
		
		ResetToDefault();
		gameObject.SetActive(false);
	}

	private void ResetToDefault() {
		_renderer.material.SetColor("_EmissionColor", _targetColor * 5);
		_light.intensity = 4.5f;
		_wasHit = false;
		_exploding = false;
		_numberOfHits = 0;
		transform.localScale = Vector3.one;	
	}

	private IEnumerator WasHit() {
		_hitTime = Time.time + HitDelayTime;
		_wasHit = true;
		var lastScale = transform.localScale;
		var lastIntensity = _light.intensity;
		while (Time.time < _hitTime) {
			var curveValue = HitScaleCurve.Evaluate((Time.time - _hitTime + HitDelayTime) / HitDelayTime);
			transform.localScale = lastScale + Vector3.one * (curveValue * curveValue) * ScaleFactor;
			_renderer.material.SetColor("_EmissionColor", _targetColor * Mathf.Lerp(30, 4, curveValue * curveValue));
			_light.intensity = lastIntensity * (curveValue * curveValue) * 1.2f;
				
			yield return null;
		}

		transform.localScale = lastScale + Vector3.one * HitScaleCurve.Evaluate(1) * ScaleFactor;
		_target = FindTarget();
		_wasHit = false;
	}
}
