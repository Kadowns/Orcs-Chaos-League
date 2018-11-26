using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour {
	
	[Range(0, 1)]
	public float Volume = 1;

	[Range(0, 255)]
	public byte Priority = 128;

	[SerializeField] private AudioClip[] _sfx;
	private AudioSource[] _audioPlayer;
	private AudioSource _loopPlayer;

	private void Awake() {
		_audioPlayer = new AudioSource[_sfx.Length];
		for(int i = 0; i < _sfx.Length; i++) {
			_audioPlayer[i] = gameObject.AddComponent<AudioSource>();
			_audioPlayer[i].priority = Priority;
			_audioPlayer[i].volume = Volume;
		}

		_loopPlayer = gameObject.AddComponent<AudioSource>();
		_loopPlayer.loop = true;
	}

	public void PlaySFxByIndex(int index, float pitch) {
		if(index >= _sfx.Length)
			return;

		_audioPlayer[index].pitch = pitch;
		_audioPlayer[index].PlayOneShot(_sfx[index]);
	}

	public void PlayForSeconds(int index, float pitch, float timeToPlay) {
		if(index >= _sfx.Length)
			return;
		
		_loopPlayer.clip = _sfx[index];
		_loopPlayer.pitch = pitch;
		_loopPlayer.Play();
		StartCoroutine(PlayForSeconds(timeToPlay));

	}

	private IEnumerator PlayForSeconds(float timeToPlay) {
		yield return new WaitForSeconds(timeToPlay);
		_loopPlayer.Stop();
	}
	
}
