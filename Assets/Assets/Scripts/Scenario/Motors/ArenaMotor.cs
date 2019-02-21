using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaMotor : ScriptableObject {

	public abstract void Setup(ArenaController controller, ArenaState state);
	public abstract void Initialize(ArenaController controller, ArenaState state);
	public abstract void Tick(ArenaController controller, ArenaState state);
	public abstract void ResetToDefault(ArenaController controller, ArenaState state);
}
