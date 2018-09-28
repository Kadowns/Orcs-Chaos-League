using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/PotatoBomb")]
public class PotatoBombEvent : GreatEvent {

	public string ObjectNameInPool;

	public void Setup(ArenaState state) {
		
	}

	protected override void OnExecute(ArenaState state) {
		int rand = Random.Range(0, state.EventSpawners.Length);
		
		state.EventSpawners[rand].Execute();
	}

	protected  override void OnTerminate(ArenaState state) {
		
	}
}
