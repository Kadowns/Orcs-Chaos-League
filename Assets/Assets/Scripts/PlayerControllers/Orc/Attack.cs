﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Attack {
    public string tag;
    public int id;
    public float range, hurtForSeconds;
    public int force;
    public bool knockBack, knockUp, miniDash, screenShake;
    public AudioClip tryClip, hitClip;
		
    private AudioSource _trySFx, _hitSFx;
		
    public void Start(GameObject gameObject) {
        _trySFx = gameObject.AddComponent<AudioSource>();
        _trySFx.clip = tryClip;
        _hitSFx =  gameObject.AddComponent<AudioSource>();
        _hitSFx.clip = hitClip;
    }

    public void TrySFx(float pitch) {
        _trySFx.pitch = pitch;
        _trySFx.Play();
    }
		
    public void HitSFx(float pitch) {
        _hitSFx.pitch = pitch;
        _hitSFx.Play();
    }
}