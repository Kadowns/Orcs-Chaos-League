using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRockBehaviour : MonoBehaviour, IThrowable {

	public float GravityScale = 10f;
	
	public int AttackerID { get; set; }

	private float _minimumTimeAlive = 0.2f;
	private float _timeAlive = 0;

	private Rigidbody _rb;
	private ScreenEffects _fx;
	private bool _collided, _canDie;
	private int _lastPlayerId = 0;

	private void Start() {
		_rb = GetComponent<Rigidbody>();

		_fx = ScreenEffects.Instance;
	}

	private void Update() {
		
		if (_canDie)
			return;
		

		_timeAlive += Time.deltaTime;
		if (_timeAlive > _minimumTimeAlive) {
			_timeAlive = 0;
			_canDie = true;
		}
	}

	private void FixedUpdate() {
		_rb.AddForce(Physics.gravity * GravityScale, ForceMode.Acceleration);
	}

	public void Throw(Vector3 dir, int attackerID) {
		AttackerID = attackerID;
		GlobalAudio.Instance.PlayByIndex(5);
		GlobalAudio.Instance.PlayByIndex(6);
		_rb.AddForce((dir + Vector3.up * 0.2f).normalized * 175, ForceMode.Impulse);
		_collided = false;	
	}



	private void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag("Lava") && _canDie) {
			_fx.CreateSmokeParticles(transform.position);
			_canDie = false;
			_collided = false;
			gameObject.SetActive(false);

		}
		else if (other.collider.CompareTag("Player") && !_collided) {
			Vector3 dir = other.transform.position - transform.position;
			var otherEntity = other.gameObject.GetComponent<MovableEntity>();
			var otherMotor = otherEntity.Motor as OrcMotor;
			var otherState = otherEntity.State as OrcEntityState;
			if (!otherState.Parrying) {
				otherMotor.Burn(otherState, 20, 1f, dir.normalized, 90, AttackerID);
				ScreenEffects.Instance.CreateRockExpParticles(transform.position);
				gameObject.SetActive(false);
			}
			else {			
				ScreenEffects.Instance.FreezeFrame(0.08f);
				Throw((transform.position - other.transform.position).normalized, AttackerID);
			}
			_fx.ScreenShake(0.1f, 0.8f);
			_fx.FreezeFrame(0.1f);
			
		}
		else if (other.collider.CompareTag("Ground")) {
			_collided = true;
		}
	}
}
