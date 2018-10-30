using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public abstract class EventContext : MonoBehaviour {

        public virtual void Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
    }


    public class SpawnContext : EventContext {
        
        public GameObject Spawner;

        public string ObjectToSpawnName;

        private EventSpawner _eventSpawner;

        private void Awake() {
            _eventSpawner = Spawner.GetComponent<EventSpawner>();
        }
        
        protected override void OnExecute() {
            _eventSpawner.SpawnObject(ObjectToSpawnName);
        }
    }

    public class FooContext : EventContext {

        public int SomeInt;
        
        protected override void OnExecute() {
            
        }
    }
}