using UnityEngine;
using Rewired;


namespace Assets.Scripts.PlayerControllers.Input {


    public class PlayerBrainMemory {
        public Player Player;
    }
    
    
    
    public static class PlayerBrains {
        public static PlayerBrainMemory[] BrainMemory = {
            new PlayerBrainMemory(),
            new PlayerBrainMemory(),
            new PlayerBrainMemory(),
            new PlayerBrainMemory()
        };
    }
}