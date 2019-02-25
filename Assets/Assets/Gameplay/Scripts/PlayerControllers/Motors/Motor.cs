using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motor : ScriptableObject {

	public abstract void Initialize(MovableEntity entity, InputController input);
	public abstract void Setup(MovableEntity entity, InputController input);
	public abstract void Tick(MovableEntity entity, InputController input);
	public abstract void FixedTick(MovableEntity entity, InputController input);
	public abstract void LateTick(MovableEntity entity, InputController input);
}
