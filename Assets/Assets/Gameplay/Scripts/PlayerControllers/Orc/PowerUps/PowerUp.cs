namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public abstract class PowerUp {

        public abstract void Initialize(OrcEntityState state);
        public abstract void Tick(OrcEntityState state);
        public abstract void Terminate(OrcEntityState state);

    }
}