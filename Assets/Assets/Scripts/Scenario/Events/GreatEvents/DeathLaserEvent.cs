using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    [CreateAssetMenu(menuName = "GreatEvent/DeathLaserEvent")]
    public class DeathLaserEvent : GreatEvent {
        
        protected override void OnExecute(ArenaState state) {
            DeathLaser.Instance.Execute();
        }

        protected override void OnTerminate(ArenaState state) {
            DeathLaser.Instance.Terminate();
        }
    }
}