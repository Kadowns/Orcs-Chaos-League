using UnityEngine;

public class SpawnPipe : Singleton<SpawnPipe> {

    public Transform PipeEnd;
    
    private ObjectPooler _pooler;


    private void Awake() {
        _pooler = ObjectPooler.Instance;
    }   
    
    public GameObject SpawnFromPool(string gameObjectToSpawn) {
        return _pooler.SpawnFromPool(gameObjectToSpawn, PipeEnd.position, Quaternion.identity);
    }
}
