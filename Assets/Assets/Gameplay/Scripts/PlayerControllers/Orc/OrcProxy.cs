using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcProxy : MonoBehaviour {

	private OrcEntityState _state;
	private Animator _anima;

	private void Awake() {
		_anima = GetComponent<Animator>();
	}
	
	private void Start () {		
		_state = GetComponentInParent<OrcEntityState>();
	}

	private void LateUpdate() {
		_anima.SetBool("Hurt", _state.Hurt);
		_anima.SetBool("IsGrounded", _state.Grounded);
		_anima.SetBool("Cooldown", _state.Cooldown);
		_anima.SetBool("Burning", _state.Burning);
		_anima.SetBool("StompDrawback", _state.StompDrawback);
		_anima.SetBool("Taunt", _state.Taunt);
		_anima.SetBool("Attack", _state.Attacking);
		_anima.SetInteger("AttackID", _state.LastAttackId);
		_anima.SetBool("Parry", _state.Parrying);
		_anima.SetBool("Counter", _state.Countered);
		_anima.SetFloat("MoveSpeed", _state.Velocity.sqrMagnitude);
		_anima.SetInteger("Direction", _state.LastDir.x < 0 ? 1 : 0);		
	}

	public void CounterEnded() {
		_state.Countered = false;
	}
}
