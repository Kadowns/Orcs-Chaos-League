using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaMotor : ScriptableObject {

	public abstract void Setup(ArenaController controller, ArenaState state);

	public virtual void Initialize(ArenaController controller, ArenaState state) {
		foreach (var t in state.Plataforms) {
			t.DefinePlataforms(state.GlobalPlatformSettings);
		}

		OnInitialize(controller, state);
	}
	protected abstract void OnInitialize(ArenaController controller, ArenaState state);
	public abstract void Tick(ArenaController controller, ArenaState state);
	public abstract void ResetToDefault(ArenaController controller, ArenaState state);
}
