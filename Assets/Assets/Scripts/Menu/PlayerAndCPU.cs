using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerAndCPU {

    new public static bool PlayerInGame;
    new public static bool CPU;

    public static class BotBrains
    {
        public static PlayerAndCPU[] PandC = {
            new PlayerAndCPU(),
            new PlayerAndCPU(),
            new PlayerAndCPU(),
            new PlayerAndCPU()
        };
    }



}
