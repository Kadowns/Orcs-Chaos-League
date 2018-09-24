using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/PotatoBomb")]
public class PotatoBombEvent : GreatEvent {

	protected override void OnExecute(ArenaState state) {
		int rand = Random.Range(0, state.EventSpawners.Length);
		state.EventSpawners[rand].ChangeEventType(EventSpawner.EventType.GuidedRock);
		state.EventSpawners[rand].Execute();
		state.EventSpawners[rand].ChangeEventType(EventSpawner.EventType.RockExplosion);
	}

	protected  override void OnTerminate(ArenaState state) {
		
	}
}
