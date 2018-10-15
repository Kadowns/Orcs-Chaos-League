using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public abstract class EventContext : MonoBehaviour {

        public virtual void Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
    }


    public class SpawnContext : EventContext {
        
        public EventSpawner Spawner;

        public string ObjectToSpawnName;
        
        protected override void OnExecute() {
            Spawner.SpawnObject(ObjectToSpawnName);
        }
    }

    public class FooContext : EventContext {
        protected override void OnExecute() {
            
        }
    }
}