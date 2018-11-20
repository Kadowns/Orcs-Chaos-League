using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events {

    public class Meteorite : MonoBehaviour, IThrowable, ISpawnable {

        public float ThrowForce = 400f;
        public int DamageOnContact = 95;

        private int _lastAttackerId = -1;
        private Rigidbody _rb;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
        }
        
        public void Throw(Vector3 dir, int attackerID) {
            _rb.AddForce(dir * ThrowForce, ForceMode.Impulse);
            _lastAttackerId = attackerID;
        }

        public void OnSpawn() {
            Throw(Vector3.down + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f) * 2.5f), _lastAttackerId);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                var entity = other.gameObject.GetComponent<MovableEntity>();
                var motor = entity.Motor as OrcMotor;
                var state = entity.State as OrcEntityState;
                motor.Burn(state, DamageOnContact, 0.5f, (entity.transform.position - transform.position).normalized, 250f, _lastAttackerId);
                ScreenEffects.Instance.FreezeFrame(0.08f);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                var plat = GetComponent<PlataformBehaviour>();
                if (plat != null) {
                    plat.Damage();
                }
            }
            
            ScreenEffects.Instance.ScreenShake(0.1f, 1f);
            ScreenEffects.Instance.CreateRockExpParticles(transform.position);
            gameObject.SetActive(false);
        }
    }
}