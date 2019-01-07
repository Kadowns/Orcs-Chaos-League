using UnityEngine;

namespace Assets.Scripts.PlayerControllers {

    [RequireComponent(typeof(Rigidbody))]
    public class DropBox : MonoBehaviour {

        private Rigidbody _rb;

        private PlayerController _controller;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            gameObject.SetActive(false);
        }

        public void ReleaseBox(Transform copyTransform, Rigidbody copyRb, PlayerController controller) {
            transform.position = copyTransform.position;
            transform.rotation = copyTransform.rotation;
            gameObject.SetActive(true);
            _rb.isKinematic = false;
            _rb.angularVelocity = copyRb.angularVelocity;
            _rb.velocity = copyRb.velocity;
            _controller = controller;
        }

        private void DestroyBox() {
            gameObject.SetActive(false);
            _rb.isKinematic = true;
            Explosion();
            ScreenEffects.Instance.CreateBoxParticles(transform.position);
            ScreenEffects.Instance.ScreenShake(0.15f, 1.5f);
            _controller.SpawnOrc(transform.position);
        }

        private void Explosion() {
            var orcs = Physics.OverlapSphere(transform.position, 20f, 1 << LayerMask.NameToLayer("Players"));
            
            foreach (var orc in orcs) {
                var entity = orc.GetComponent<MovableEntity>();
                var motor = entity.Motor as OrcMotor;
                var state = entity.State as OrcEntityState;
                if (state.Controller != _controller)
                    motor.Damage(state, (orc.transform.position - transform.position).normalized, 200f, 0.5f, true, true, _controller.PlayerNumber);
            }
        }
        
        private void OnCollisionEnter(Collision other) {
            DestroyBox();
        }
    }
}