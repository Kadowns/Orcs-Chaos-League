using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : Singleton<GlobalAudio> {

	[SerializeField] private AudioClip[] _sfx;
	

	private AudioSource _sfxPlayer;
	
	private AudioSource _ambPlayer;


	
	private void Awake() {
		_sfxPlayer = gameObject.AddComponent<AudioSource>();
	
		_ambPlayer = gameObject.AddComponent<AudioSource>();
		
	}

	public void StopLoop() {
		_ambPlayer.Stop();
	}

	public void PlayByIndex(int index) {
		if(index > _sfx.Length)
			return;

		_sfxPlayer.PlayOneShot(_sfx[index]);
	}
	public void PlayByIndex(int index, float pitch) {
		if(index > _sfx.Length)
			return;

		_sfxPlayer.pitch = pitch;
		_sfxPlayer.PlayOneShot(_sfx[index]);
	}

	public void LoopByIndex(int index) {
		if(index > _sfx.Length)
			return;

		_ambPlayer.loop = true;
		_ambPlayer.clip = _sfx[index];
		_ambPlayer.Play();
	}
}
