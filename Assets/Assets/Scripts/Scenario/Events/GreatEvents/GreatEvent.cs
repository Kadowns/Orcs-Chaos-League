﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GreatEvent : ScriptableObject {

    public Sprite EventHudSprite;

    [Range(10f, 60f)]
    public float Duration;

    public virtual void Setup(ArenaState state) {}

    public void Execute(ArenaState state) {
        EventHUDProxy.Instance.ShowEventHUD(EventHudSprite);
        var fx = ScreenEffects.Instance;
        fx.ScreenShake(1.5f, 5f);
        fx.BlurForSeconds(1.5f, 10f, new Color(0.85f, 0.85f, 0.85f), new Color(1f, 0.85f, 0.85f));
        var music = MusicController.Instance;
        music.SetBGMPitch(1.075f);
        music.DramaticFrequencyChange(0.1f, 1.5f, 0.5f, 200f, 22000f);
             
        OnExecute(state);
    }
    protected abstract void OnExecute(ArenaState state);

    public void Terminate(ArenaState state) {
        EventHUDProxy.Instance.HideEventHUD();
        ScreenEffects.Instance.Blur(0, Color.white);
        MusicController.Instance.SetBGMPitch(1f);
        OnTerminate(state);
    }
    protected abstract void OnTerminate(ArenaState state);
}