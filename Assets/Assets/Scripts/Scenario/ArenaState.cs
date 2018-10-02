using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaState : MonoBehaviour {
    public ArenaMotor Motor;
    public List<PlataformBehaviour> Plataforms;
    public List<GreatEvent> GreatEvents;
    public LavaGeyser[] LavaGeysers;
    public AnimationCurve OscilationCurve;	
    public int MaxPlataformHits;
    public float PlataformLoweredTime;
    public float OscilationFrequency;
    public float OscilationScale;
    public bool RandomOffSet;

}
