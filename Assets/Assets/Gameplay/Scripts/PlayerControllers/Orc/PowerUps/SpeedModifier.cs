namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public class SpeedModifier : PowerUp {
        
        private float m_modifier;

        public SpeedModifier(float modifier) {
            m_modifier = modifier;
        }

        public override void Initialize(OrcEntityState state) {            
            state.MoveSpeed *= m_modifier;
        }

        public override void Tick(OrcEntityState state) { }

        public override void Terminate(OrcEntityState state) {
            state.MoveSpeed /= m_modifier;
        }
    }
}