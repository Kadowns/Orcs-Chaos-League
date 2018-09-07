using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour {

	public float WaterLevel;
	public float Boyance = 400;
	
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			var gravity = other.GetComponent<CustomGravity>();
			gravity.GlobalGravity /= 80;
		}
	}
	
	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			var gravity = other.GetComponent<CustomGravity>();
			gravity.GlobalGravity *= 80;
		}
	}
}
