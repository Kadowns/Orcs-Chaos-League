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
            Throw(Vector3.down + new Vector3(Random.Range(-.5f, .5f), -1, Random.Range(-.5f, .5f) * 2.5f), _lastAttackerId);
            _rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(-180f, 180f));
        }

        private void OnCollisionEnter(Collision other) {
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                var plat = other.collider.GetComponent<PlataformBehaviour>();
                if (plat != null) {
                    plat.Damage();
                }
            }

            var cols = Physics.OverlapSphere(transform.position, 10, 1 << LayerMask.NameToLayer("Players"));

            if (cols.Length > 0) {
                foreach (var col in cols) {
                    var entity = col.gameObject.GetComponent<MovableEntity>();
                    var motor = entity.Motor as OrcMotor;
                    var state = entity.State as OrcEntityState;
                    motor.Burn(state, DamageOnContact, 0.5f, (entity.transform.position - transform.position).normalized, 250f, _lastAttackerId);
                }
            }
            
            
            ScreenEffects.Instance.ScreenShake(0.1f, 1f);
            ScreenEffects.Instance.CreateMeteoriteParticles(transform.position);
            gameObject.SetActive(false);
        }
    }
}