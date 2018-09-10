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
		for (int i = 0; i < state.Plataforms.Length; i++) {
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

	public override void OnSuddenDeath(ArenaController controller, ArenaState state) {
		controller.StartCoroutine(SuddenDeath(controller, state));
	}

	protected IEnumerator SuddenDeath(ArenaController controller, ArenaState state) {

		controller.StartCoroutine(SuddenDeathExplosions(controller, state));
		int rand = Random.Range(0, state.Plataforms.Length);
		int sunkPlataforms = 0;
		bool[] sunk = new bool[state.Plataforms.Length];
		do {

			int plataformsToSink = state.Plataforms.Length - (int)(state.Plataforms.Length * ((float)(controller.PlayerInGame - 1) / 4));
			
			if (sunkPlataforms < plataformsToSink) {
				state.Plataforms[rand].Lower();		
				sunk[rand] = true;
				sunkPlataforms++;
				rand = rand + 1 >= state.Plataforms.Length ? 0 : rand + 1;
				yield return new WaitForSeconds(0.5f);
			}
			else {

				int i = rand;
				do {

					if (sunk[i]) {
						state.Plataforms[i].Raise();
						sunkPlataforms--;
						sunk[i] = false;
						yield return new WaitForSeconds(2f);
						break;
					}

					i = i + 1 >= state.Plataforms.Length ? 0 : i + 1;
				} while (true);
			}

		} while (controller.SuddenDeath);
	}

	private IEnumerator RaisePlataforms(ArenaController controller, ArenaState state) {
		yield return null;
		foreach (var platforms in state.Plataforms) {
			platforms.Raise();
		}
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

