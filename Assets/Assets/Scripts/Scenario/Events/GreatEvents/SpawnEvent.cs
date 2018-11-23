using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
	[CreateAssetMenu(menuName = "GreatEvent/SpawnEvent")]
	public class SpawnEvent : GreatEvent {


		public string ObjectToSpawnName;
		public string SpawnTypeName;
		public int ItemsToSpawn = 5;
		public float SpawnInterval = 1.5f;

		private SpawnerManager _spawnManager;

		private Coroutine _eventRoutine;
		
		public override void Setup(ArenaState state) {
			_spawnManager = SpawnerManager.Instance;
		}

		protected override void OnExecute(ArenaState state) {
			_eventRoutine = state.StartCoroutine(DoEvent());
		}

		protected  override void OnTerminate(ArenaState state) {
			if (_eventRoutine != null) {
				state.StopCoroutine(_eventRoutine);
			}
		}

		private IEnumerator DoEvent() {
			for (int i = 0; i < ItemsToSpawn; i++) {
				_spawnManager.SpawnObjects(ObjectToSpawnName,SpawnTypeName);
				yield return new WaitForSeconds(SpawnInterval);
			}
		}

		private void OnValidate() {
			if (ItemsToSpawn < 1) {
				ItemsToSpawn = 1;
			}

			if (SpawnInterval < 0.1f) {
				SpawnInterval = 0.1f;
			}
		}
	}
}
