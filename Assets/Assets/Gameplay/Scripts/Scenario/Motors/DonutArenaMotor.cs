using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ArenaMotor/Donut")]
public class DonutArenaMotor : ArenaMotor {
	
	public override void Setup(ArenaController controller, ArenaState state) {
		
	}

	public override void Initialize(ArenaController controller, ArenaState state) {
//		for (int i = 0; i < state.Plataforms.Length; i++) {
//			state.Plataforms[i].DefinePlataforms(state.MaxPlataformHits, state.PlataformLoweredTime, state.OscilationFrequency,
//				state.OscilationScale, state.OscilationCurve, (float)i / state.Plataforms.Length);
//		}
	}

	public override void Tick(ArenaController controller, ArenaState state) {
	}

	public override void ResetToDefault(ArenaController controller, ArenaState state) {
	}

	public void GreatEvents(ArenaController controller, ArenaState state) {
	}
}
