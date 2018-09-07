using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaState : MonoBehaviour {
    public ArenaMotor Motor;
    public PlataformBehaviour[] Plataforms;
    public LavaExplosion[] LavaExplosions;
    public AnimationCurve OscilationCurve;	
    public int MaxPlataformHits;
    public float PlataformLoweredTime;
    public float OscilationFrequency;
    public float OscilationScale;
    public bool RandomOffSet;

    private void OnValidate() {
        for (int i = 0; i < Plataforms.Length; i++) {
            if (Plataforms[i] != null)
            Plataforms[i].DefinePlataforms(MaxPlataformHits, PlataformLoweredTime, OscilationFrequency,
                OscilationScale, OscilationCurve, RandomOffSet ? Random.Range(0f, 1f) : (float)i / Plataforms.Length);
        }
    }
}
