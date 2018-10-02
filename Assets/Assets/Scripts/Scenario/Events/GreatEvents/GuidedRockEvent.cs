using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/GuidedRock")]
public class GuidedRockEvent : GreatEvent {

    [Range(2, 15)]
    public int RocksToSpawn = 5;
    [Range(0.1f, 10f)]
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
