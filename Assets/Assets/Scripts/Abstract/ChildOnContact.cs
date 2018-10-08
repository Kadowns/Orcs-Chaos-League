using UnityEngine;

namespace Assets.Scripts.Abstract {
    [RequireComponent(typeof(Collider))]
    public class ChildOnContact : MonoBehaviour {

        public LayerMask MaskToContact;
        
        private float _sizeY;
        
        private void Awake() {
            _sizeY = GetComponent<Collider>().bounds.size.y / 2;
        }

        private void FixedUpdate() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _sizeY + 0.1f, MaskToContact)) {
                transform.SetParent(hit.transform);
            }
            else {
                transform.SetParent(null);
            }
        }
    }
}