using UnityEngine;

namespace Assets.Scripts.PlayerControllers.Input {

    public class BotBrainMemory {
        public Transform ThisOrc { get; set; }
        public Transform ThisBox { get; set; }

        public Transform Target;

        public float DistanceToTarget = 0;
        
        public float InputTimer = 0;

        public float NextInputTime {
            get { return 0.15f; }
        }
    }
    
    public static class BotBrains {
        public static readonly BotBrainMemory[] BotBrainMemories = {
            new BotBrainMemory(),
            new BotBrainMemory(),
            new BotBrainMemory(),
            new BotBrainMemory()
        };
    }
}