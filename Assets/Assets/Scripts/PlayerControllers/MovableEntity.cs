using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MovableEntity : MonoBehaviour {

	[HideInInspector] public InputController Input;
	public EntityState State;
	public Motor Motor;
	
	private void Awake() {
		Motor.Initialize(this, Input);
	}

	private void Start() {
		Motor.Setup(this, Input);
	}

	private void Update() {
		Motor.Tick(this, Input);
	}
	
	private void FixedUpdate() {
		Motor.FixedTick(this, Input);
	}
	
	private void LateUpdate() {
		Motor.LateTick(this, Input);
	}
}
