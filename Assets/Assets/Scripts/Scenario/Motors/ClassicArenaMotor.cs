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
		int rand = Random.Range(0, 3);
		int sunkPlataforms = 0;
		bool[] sunk = new bool[4];
		do {
			int plataformsToSink = 4 - (controller.PlayerInGame - 2);
			if (sunkPlataforms < plataformsToSink) {
				while (state.Plataforms[rand].transform.position.y > -8) {
					state.Plataforms[rand].transform.position += Vector3.down * 4 * Time.deltaTime;

					yield return null;
				}

				state.Plataforms[rand].transform.position = new Vector3(state.Plataforms[rand].transform.position.x, -8, state.Plataforms[rand].transform.position.z);
				sunk[rand] = true;
				sunkPlataforms++;
				rand = rand + 1 >= state.Plataforms.Length ? 0 : rand + 1;
			}
			else {

				int i = rand;
				do {

					if (sunk[i]) {
						while (state.Plataforms[i].transform.position.y < 0) {
							state.Plataforms[i].transform.position += Vector3.up * 4 * Time.deltaTime;

							yield return null;
						}

						state.Plataforms[i].transform.position =
							new Vector3(state.Plataforms[i].transform.position.x, 0, state.Plataforms[i].transform.position.z);
						sunkPlataforms--;
						sunk[i] = false;
						yield return new WaitForSeconds(10f);
						break;
					}

					i = i + 1 >= state.Plataforms.Length ? 0 : i + 1;
				} while (true);
			}

		} while (controller.SuddenDeath);
	}

	private IEnumerator RaisePlataforms(ArenaController controller, ArenaState state) {
		yield return null;
		for (int i = 0; i < state.Plataforms.Length; i++) {
			while (state.Plataforms[i].transform.position.y < 0) {
				state.Plataforms[i].transform.position += Vector3.up * 4 * Time.deltaTime;
				yield return null;
			}

			state.Plataforms[i].transform.position = new Vector3(state.Plataforms[i].transform.position.x, 0, state.Plataforms[i].transform.position.z);
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

