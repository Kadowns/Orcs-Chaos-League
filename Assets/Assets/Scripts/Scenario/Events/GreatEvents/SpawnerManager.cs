using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenario.Events.GreatEvents {
    
    public class SpawnerManager : Singleton<SpawnerManager> {

        [HideInInspector]
        public List<EventSpawner> Spawners;

        public void SpawnObjects(string name) {
            while (!Spawners[Random.Range(0, Spawners.Count - 1)].SpawnObject(name)) { }
        }
    }
}
