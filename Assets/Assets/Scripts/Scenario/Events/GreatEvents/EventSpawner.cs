using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public class EventSpawner : MonoBehaviour {

        [SerializeField] private Transform _spawnTransform;

        private ObjectPooler _pooler;
        
        private Animator _animator;

        private string _objectToSpawnName;

        private void Awake() {
            _pooler = ObjectPooler.Instance;
            _animator = GetComponent<Animator>();
        }

        public void SpawnObject(string objectName) {
            _objectToSpawnName = objectName;
            _animator.SetTrigger("StartSpawnAnimation");
        }

        private void OnSpawnAnimationFinished() {
            _pooler.SpawnFromPool(_objectToSpawnName, _spawnTransform.position, Quaternion.identity);
            _animator.SetTrigger("StopSpawnAnimation");
        }

    }
}