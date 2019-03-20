using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Scenario.Events.GreatEvents;
using UnityEngine;

public class ArenaState : MonoBehaviour {

    
    public bool IndividualPlatformEditing { get; set; }
    public ArenaMotor Motor;
    public List<LavaGeyser> LavaGeysers;
    public PlataformBehaviour.PlataformSettings GlobalPlatformSettings = new PlataformBehaviour.PlataformSettings();
    public SpawnerManager SpawnerManager;
    public List<PlataformBehaviour> Plataforms;
    public List<GreatEvent> GreatEvents;
    

}
