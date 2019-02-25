using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour {
	private void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag("Player")) {
			var otherEntity = other.collider.GetComponent<MovableEntity>();
			var otherMotor = otherEntity.Motor as OrcMotor;
			var otherState = otherEntity.State as OrcEntityState;
			otherMotor.FellOnLava(otherState);
		}
	}
}
