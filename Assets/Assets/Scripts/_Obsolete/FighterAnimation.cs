using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimation : MonoBehaviour {

	private FighterBehaviour _fighter;
	private Animator _anima;

	private void Awake() {
		_anima = GetComponent<Animator>();
	}
	
	private void Start () {		
		_fighter = GetComponentInParent<FighterBehaviour>();
	}

	private void LateUpdate() {
		_anima.SetBool("Hurt", _fighter.IsHurt());
		_anima.SetBool("IsGrounded", _fighter.IsGrounded());
		_anima.SetBool("Cooldown", _fighter.Cooldown());
		_anima.SetBool("Burning", _fighter.IsBurning());
		_anima.SetBool("StompDrawback", _fighter.IsDrawbacked());
		_anima.SetBool("Taunt", _fighter.IsTaunting());
		_anima.SetBool("Attack", _fighter.IsAttacking());
		_anima.SetInteger("AttackID", _fighter.GetAttackId());
		_anima.SetBool("Parry", _fighter.IsParrying());
		_anima.SetFloat("MoveSpeed", _fighter.Velocity.sqrMagnitude);
		_anima.SetInteger("Direction", _fighter.LastDir().x < 0 ? 1 : 0);		
	}
}
