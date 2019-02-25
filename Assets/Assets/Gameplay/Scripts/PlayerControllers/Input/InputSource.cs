using UnityEngine;

namespace Assets.Scripts.PlayerControllers.Input {
    
    public abstract class InputSource : ScriptableObject {
        
        public abstract void Tick(InputController input);
        
    }
}