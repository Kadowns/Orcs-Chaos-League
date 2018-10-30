using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/PipeSpawnEvent")]
public class PipeSpawnEvent : GreatEvent {


	public string ObjectToSpawnName;
	public int ItemsToSpawn = 5;
	public float SpawnInterval = 1.5f;
	
	private SpawnPipe _pipe;
	
	public override void Setup(ArenaState state) {
		_pipe = SpawnPipe.Instance;
	}

	protected override void OnExecute(ArenaState state) {
		_pipe.SpawnObjects(ObjectToSpawnName, ItemsToSpawn, SpawnInterval);
	}

	protected  override void OnTerminate(ArenaState state) {
		_pipe.ForceStop();
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
