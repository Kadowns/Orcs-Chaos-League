namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public class ExtraJump : PowerUp {
        
        public override void Initialize(OrcEntityState state) {
            state.MaxAirJumps++;
        }

        public override void Tick(OrcEntityState state) {
            
        }

        public override void Terminate(OrcEntityState state) {
            state.MaxAirJumps--;
        }
    }
}