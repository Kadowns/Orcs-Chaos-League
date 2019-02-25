namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public class StompModifier : PowerUp {

        private float m_modifer;
        
        public StompModifier(float modifier) {
            m_modifer = modifier;
        }
        
        public override void Initialize(OrcEntityState state) {
            state.DropAttackGravity *= m_modifer;
            state.DropAttackForceMultiplier *= m_modifer;
        }

        public override void Tick(OrcEntityState state) {
            
        }

        public override void Terminate(OrcEntityState state) {
            state.DropAttackGravity /= m_modifer;
            state.DropAttackForceMultiplier /= m_modifer;
        }
    }
}