using System;
using UnityEngine;

namespace Assets.Scripts.PlayerControllers.Items {
    [RequireComponent(typeof(Collider))]
    public class ProxyCollider : MonoBehaviour {

        public event Action<Collider> OnProxyTriggerEnter;
        public event Action<Collider> OnProxyTriggerStay;
        public event Action<Collider> OnProxyTriggerExit;

        public event Action<Collision> OnProxyCollisionEnter;
        public event Action<Collision> OnProxyCollisionStay;
        public event Action<Collision> OnProxyCollisionExit;

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
