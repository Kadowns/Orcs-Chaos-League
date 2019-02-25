using System.Collections;
using System.Collections.Generic;
using OCL;
using UnityEngine;

[System.Serializable] 
public class Attack {
    public string tag;
    public int id;
    public float range, hurtForSeconds;
    public int force;
    public bool knockBack, knockUp, miniDash, screenShake;
    public AudioClip tryClip, hitClip;
		
    public void TrySFx(float pitch) {
        AudioController.Instance.Play(
            tryClip,
            AudioController.SoundType.ExclusiveSoundEffect,
            false,
            pitch,
            200
        );
    }
		
    public void HitSFx(float pitch) {
        AudioController.Instance.Play(
            hitClip,
            AudioController.SoundType.ExclusiveSoundEffect,
            false,
            pitch,
            255
        );
    }
}
