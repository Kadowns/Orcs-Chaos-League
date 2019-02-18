using System;
using UnityEngine;

namespace Assets.Scripts.PlayerControllers.Orc.PowerUps {
    
    public class Shield : PowerUp {

        private Action<int> m_deathCallback; 
        
        public override void Initialize(OrcEntityState state) {            
            state.HasShield = true;
            state.ShieldConsumed = false;
            m_deathCallback =  (attacker) => { state.ShieldConsumed = false; };
            state.Controller.OnDeath += m_deathCallback;
        }

        public override void Tick(OrcEntityState state) { }

        public override void Terminate(OrcEntityState state) {
            state.Controller.OnDeath -= m_deathCallback;
            state.HasShield = false;
            state.ShieldConsumed = false;
        }
    }
}