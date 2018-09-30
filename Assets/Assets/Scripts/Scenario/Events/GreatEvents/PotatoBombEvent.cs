using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/PotatoBomb")]
public class PotatoBombEvent : GreatEvent {

	public int BombsToSpawn = 5;
	public float SpawnInterval = 1.5f;
	
	private SpawnPipe _pipe;
	
	public override void Setup(ArenaState state) {
		_pipe = SpawnPipe.Instance;
	}

	protected override void OnExecute(ArenaState state) {
		_pipe.SpawnObjects("PotatoBomb", BombsToSpawn, SpawnInterval);
	}

	protected  override void OnTerminate(ArenaState state) {
		_pipe.ForceStop();
	}
}
