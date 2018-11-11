using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public class SpawnerManager : Singleton<SpawnerManager> {

        [System.Serializable]
        public class SpawnerType {
            public string Name;
            public List<EventSpawner> Spawners;
        }

        public List<SpawnerType> SpawnersTypes;
        public Dictionary<string, List<EventSpawner>> Map;

        private void Awake() {
            Map = new Dictionary<string, List<EventSpawner>>();

            foreach (var type in SpawnersTypes) {
                Map[type.Name] = type.Spawners;
            }
            
        }

        public void SpawnObjects(string objectName, string spawnerType) {
            while (!Map[spawnerType][Random.Range(0, SpawnersTypes.Count - 1)].SpawnObject(objectName)) { }
        }
    }
}
