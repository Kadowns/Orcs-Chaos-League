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
			state.Plataforms[i].DefinePlataforms(state.MaxPlataformHits, state.PlataformLoweredTime, state.OscilationFrequency,
				state.OscilationScale, state.OscilationCurve, Random.Range(0f, 1f));
		}
	}

	public override void Tick(ArenaController controller, ArenaState state) { }

	public override void ResetToDefault(ArenaController controller, ArenaState state) {
		controller.StopAllCoroutines();
		controller.StartCoroutine(RaisePlataforms(controller, state));
	}

	public override void NormalEvents(ArenaController controller, ArenaState state) {
		controller.StartCoroutine(Explosions(controller, state));
	}	


	protected IEnumerator Explosions(ArenaController controller, ArenaState state) {
		yield return null;
		do {
			int explosions = Random.Range(2, 6);
			for (int i = 0; i < explosions; i++) {
				int rand = Random.Range(0, state.Events.Length);
				state.Events[rand].Execute();
				yield return new WaitForSeconds(Random.Range(1, 5));
			}

			yield return new WaitForSeconds(10f);
		} while (!controller.SuddenDeath);
	}

	protected IEnumerator SuddenDeathExplosions(ArenaController controller, ArenaState state) {
		yield return null;
		do {
			int randomizer = Random.Range(4, 10);
			for (int i = 0; i < randomizer; i++) {
				int rand = Random.Range(0, state.Events.Length);
				state.Events[rand].Execute();
				yield return new WaitForSeconds(5f / randomizer);
			}

			yield return new WaitForSeconds(7f);
		} while (controller.SuddenDeath);
	}
}

