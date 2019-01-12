using UnityEngine;

namespace Assets.Scripts.PlayerControllers.Items {
    [RequireComponent(typeof(Collider))]
    public class ProxyCollider : MonoBehaviour {

        public delegate void TriggerDelegate(Collider other);

        public delegate void CollisionDelegate(Collision collision);

        public event TriggerDelegate OnProxyTriggerEnter;
        public event TriggerDelegate OnProxyTriggerStay;
        public event TriggerDelegate OnProxyTriggerExit;

        public event CollisionDelegate OnProxyCollisionEnter;
        public event CollisionDelegate OnProxyCollisionStay;
        public event CollisionDelegate OnProxyCollisionExit;

        private void OnTriggerEnter(Collider other) {
            if (OnProxyTriggerEnter != null) {
                OnProxyTriggerEnter(other);
            }
        }

        private void OnTriggerStay(Collider other) {
            if (OnProxyTriggerStay != null) {
                OnProxyTriggerStay(other);
            }
        }
        
        private void OnTriggerExit(Collider other) {
            if (OnProxyTriggerExit != null) {
                OnProxyTriggerExit(other);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (OnProxyCollisionEnter != null) {
                OnProxyCollisionEnter(other);
            }
        }
        
        private void OnCollisionStay(Collision other) {
            if (OnProxyCollisionStay != null) {
                OnProxyCollisionStay(other);
            }
        }
        
        private void OnCollisionExit(Collision other) {
            if (OnProxyCollisionExit != null) {
                OnProxyCollisionExit(other);
            }
        }
    }
}
