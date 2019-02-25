using Assets.Scripts.PlayerControllers.Items;
using OCL;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    public class DeathLaser : Singleton<DeathLaser> {

        public float MaxRotationSpeed = 45;
        
        public ParticleSystem[] ParticleSystems = new ParticleSystem[2];

        public ProxyCollider ProxyCollider;

        public AudioClip LaserIntro, LaserLoop;

        public AnimationCurve[] SpeedCurves;

        private AnimationCurve _actualSpeedCurve;
        
        private Animator _animator;

        private AudioChannel _channel;

        private bool _executing;

        private float _startTime;
        
        private void Awake() {
            Instance = this;
            _animator = GetComponent<Animator>();
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            ProxyCollider.OnProxyTriggerEnter += OnTrigger;
        }

        private void OnDisable() {
            ProxyCollider.OnProxyTriggerEnter -= OnTrigger;
        }

        private void OnTrigger(Collider other) {
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Players")) {
                var entity = other.GetComponent<MovableEntity>();
                var motor = entity.Motor as OrcMotor;
                var state = entity.State as OrcEntityState;
                motor.Burn(state, 100, 0.5f, Vector3.up, 300f, -1);
            }
        }

        public void Execute() {
            gameObject.SetActive(true);
            _animator.SetTrigger("Execute");
        }

        private void OnExecuted() {
            _executing = true;
            _channel = AudioController.Instance.Play(LaserIntro, AudioController.SoundType.GlobalSoundEffect, true);
            _startTime = Time.time;
            _actualSpeedCurve = SpeedCurves[Random.Range(0, SpeedCurves.Length)];
            ParticleSystems[0].Play();
            ParticleSystems[1].Play();
        }

        private void Update() {
            if (!_executing) return;
            
            if (!_channel.IsPlaying) {
                _channel.Loop = true;
                _channel.Play(LaserLoop);
            }

            if (_channel.Loop) {
                _channel.Pitch = _actualSpeedCurve.Evaluate(Time.time - _startTime);
            }
        }

        private void FixedUpdate() {
            if (_executing) {
                transform.Rotate(Vector3.up, MaxRotationSpeed * _actualSpeedCurve.Evaluate(Time.time - _startTime) * Time.fixedDeltaTime);
            }
        }

        public void Terminate() {
            _executing = false;
            _channel.Stop();
            _channel.IsPrivate = false;
            ParticleSystems[0].Stop();
            ParticleSystems[1].Stop();
            _animator.SetTrigger("Terminate");
        }

        public void OnTerminated() {
            gameObject.SetActive(false);
        }       
    }
}