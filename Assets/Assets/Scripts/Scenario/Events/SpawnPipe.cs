using System.Collections;
using UnityEngine;

public class SpawnPipe : Singleton<SpawnPipe> {

    [SerializeField] private Transform _pipeEnd;
    
    private ObjectPooler _pooler;

    private Coroutine _spawnObjectsRoutine;
    
    private Animator _animator;

    private bool _readyToSpawn;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _pooler = ObjectPooler.Instance;
    }   
    
    public void SpawnObjects(string objName, int quantity, float spawnInterval) {
        if (_spawnObjectsRoutine != null)
            StopCoroutine(_spawnObjectsRoutine);
        _spawnObjectsRoutine = StartCoroutine(DoSpawnObjects(objName, quantity, spawnInterval));
    }

    private IEnumerator DoSpawnObjects(string objName, int quantity, float spawnInterval) {
        if (!_readyToSpawn)
            yield return StartCoroutine(LowerPipe());

        for (int i = 0; i < quantity; i++) {
            _pooler.SpawnFromPool(objName, _pipeEnd.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        _readyToSpawn = false;
        _animator.SetTrigger("RaisePipe");
    }

    private IEnumerator LowerPipe() {
        _animator.SetTrigger("LowerPipe");
        while (!_readyToSpawn) {
            yield return null;
        }
    }

    public void ForceStop() {
        if (_spawnObjectsRoutine != null)
            StopCoroutine(_spawnObjectsRoutine);
    }

    private void FinishedLowerAnimation() {
        _readyToSpawn = true;
    }
    
}
