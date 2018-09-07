using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByDuration : MonoBehaviour {

	public void DestroyAfterSeconds(float duration) {
		Destroy(gameObject, duration);
	}	

	public void DestroyAfterFinished() {
		var ps = GetComponent<ParticleSystem>();
		Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constant);
	}

	public void InactiveAfterFineshed() {
		var ps = GetComponent<ParticleSystem>();
		StartCoroutine(SetInactiveAfterSeconds(ps.main.duration + ps.main.startLifetime.constant));
	}

	public void InactiveAfterSeconds(float seconds) {
		StartCoroutine(SetInactiveAfterSeconds(seconds));
	}

	IEnumerator SetInactiveAfterSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		gameObject.SetActive(false);
	}
}
