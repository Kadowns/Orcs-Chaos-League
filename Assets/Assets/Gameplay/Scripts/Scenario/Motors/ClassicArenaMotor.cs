using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ArenaMotor/Classic")]
public class ClassicArenaMotor : ArenaMotor {

	public override void Setup(ArenaController controller, ArenaState state) {
		
	}

	public override void Initialize(ArenaController controller, ArenaState state) {
		for (int i = 0; i < state.Plataforms.Count; i++) {
			state.Plataforms[i].DefinePlataforms(state.GlobalPlatformSettings);
		}
	}

	public override void Tick(ArenaController controller, ArenaState state) { }

	public override void ResetToDefault(ArenaController controller, ArenaState state) {
		controller.StopAllCoroutines();
	}
}

