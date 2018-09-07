using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motor : ScriptableObject {

	public abstract void Initialize(MovableEntity entity, InputSource input);
	public abstract void Setup(MovableEntity entity, InputSource input);
	public abstract void Tick(MovableEntity entity, InputSource input);
	public abstract void FixedTick(MovableEntity entity, InputSource input);
	public abstract void LateTick(MovableEntity entity, InputSource input);
}
