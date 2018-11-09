using UnityEngine;

namespace Assets.Scripts.Scenario.Events {
    public class LavaWave : MonoBehaviour, ISpawnable {

        public float LifeTime = 5;
        public float Speed = 20;

        private float _timeAlive = 0;

        private Vector3 _direction;

        private void Awake() {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void Update() {
            transform.position += _direction * Time.deltaTime * Speed;
            if (Time.time > _timeAlive) {
                gameObject.SetActive(false);
            }
        }

        public void OnSpawn() {
            _direction = -transform.position.normalized;
            _timeAlive = Time.time + LifeTime;   
        }
    }
}