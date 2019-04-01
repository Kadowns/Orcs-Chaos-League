using UnityEngine;

namespace Assets.Gameplay.Scripts.Scenario.Motors {
    
    [CreateAssetMenu(menuName = "ArenaMotor/Ring")]
    public class RingArenaMotor : ArenaMotor {


        private float m_timeRotating, m_rotateTime = 10;
        
        private Transform m_ring;
        private Quaternion m_targetRotation, m_lastRotation;

        private Vector3 m_lastEulerRotation;
        public override void Setup(ArenaController controller, ArenaState state) {
            
        }

        protected override void OnInitialize(ArenaController controller, ArenaState state) {
            m_ring = ((RingArenaState) state).Ring;
            SetNextRotation(m_lastEulerRotation = new Vector3(180, 0, 0));
            
        }

        public override void Tick(ArenaController controller, ArenaState state) {
            m_timeRotating += Time.deltaTime;
            if (m_timeRotating > m_rotateTime) {
                m_timeRotating = 0;
                SetNextRotation(m_lastEulerRotation = new Vector3(m_lastEulerRotation.x + 180 * Random.Range(-1, 1), 0, 0));
            }
            m_ring.rotation = Quaternion.Slerp(m_lastRotation, m_targetRotation, m_timeRotating / m_rotateTime);
        }

        public override void ResetToDefault(ArenaController controller, ArenaState state) {
            
        }

        private void SetNextRotation(Vector3 euler) {
            m_targetRotation = Quaternion.Euler(euler);
            m_lastRotation = m_ring.rotation;
        }
    }
}