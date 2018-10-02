using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/GuidedRock")]
public class GuidedRockEvent : GreatEvent {

    public int RocksToSpawn = 5;
    public float SpawnInterval = 1.5f;
    
    private SpawnPipe _pipe;
	
    public override void Setup(ArenaState state) {
        _pipe = SpawnPipe.Instance;
    }
    
    protected override void OnExecute(ArenaState state) {
        _pipe.SpawnObjects("GuidedRock", RocksToSpawn, SpawnInterval);
    }

    protected override void OnTerminate(ArenaState state) {
        _pipe.ForceStop();
    }
}
