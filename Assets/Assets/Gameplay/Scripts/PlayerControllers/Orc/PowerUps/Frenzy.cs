namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public class Frenzy : PowerUp {
        public override void Initialize(OrcEntityState state) {
            
        }

        public override void Tick(OrcEntityState state) {
            state.AttackSpeed = 0.02f;
        }

        public override void Terminate(OrcEntityState state) {
            state.AttackSpeed = state.InitialAttackSpeed;
        }
    }
}