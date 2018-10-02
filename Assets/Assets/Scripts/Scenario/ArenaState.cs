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

    private void OnValidate() {
        for (int i = 0; i < Plataforms.Count; i++) {
            if (Plataforms[i] != null)
            Plataforms[i].DefinePlataforms(MaxPlataformHits, PlataformLoweredTime, OscilationFrequency,
                OscilationScale, OscilationCurve, RandomOffSet ? Random.Range(0f, 1f) : (float)i / Plataforms.Count);
        }
    }
}
