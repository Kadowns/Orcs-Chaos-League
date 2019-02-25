using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorProxy : MonoBehaviour {

	private Animator _animator;
	
	private void Awake() {
		_animator = GetComponent<Animator>();
	}
	
	public void StopAnimator() {
		_animator.enabled = false;
	}
	
	public void StartAnimator() {
		_animator.enabled = true;
	}
}
