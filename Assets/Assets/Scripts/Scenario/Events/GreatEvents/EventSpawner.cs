using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public class EventSpawner : MonoBehaviour {

        [SerializeField] private Transform _spawnTransform;

        private ObjectPooler _pooler;
        
        private Animator _animator;

        private string _objectToSpawnName;

        private bool _spawning;

        private void Awake() {
            _pooler = ObjectPooler.Instance;
            _animator = GetComponent<Animator>();
        }

        public bool SpawnObject(string objectName) {
            if (_spawning)
                return false;
            
            _objectToSpawnName = objectName;
            _animator.SetTrigger("Raise");
            _spawning = true;
            return true;
        }

        private void OnSpawnAnimationFinished() {
            _pooler.SpawnFromPool(_objectToSpawnName, _spawnTransform.position, Quaternion.identity);
            _pooler.SpawnFromPool("ObjectSpawnPS", _spawnTransform.position, Quaternion.identity);
            _animator.SetTrigger("Lower");
            _spawning = false;
        }
    }
}