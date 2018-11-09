using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public class SpawnerManager : Singleton<SpawnerManager> {

        [HideInInspector]
        public Dictionary<string, List<EventSpawner>> Spawners = new Dictionary<string, List<EventSpawner>>();

        public void SpawnObjects(string objectName, string spawnerType) {
            while (!Spawners[spawnerType][Random.Range(0, Spawners.Count - 1)].SpawnObject(name)) { }
        }
    }
}
