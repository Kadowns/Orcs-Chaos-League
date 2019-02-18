// using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class MusicController : Singleton<MusicController> {
//	
//	[SerializeField] private AudioClip[] _bgm;
//	
//	[Range(0f, 1f)]
//	public float Volume;
//
//	[Range(0, 255)] public byte Priotity = 128;
//	
//	private AudioSource _bgmPlayer;
//	
//	private AudioLowPassFilter _bgmLowPassFilter;
//
//
//
//	// Use this for initialization
//	private void Awake() {
//		_bgmPlayer = gameObject.AddComponent<AudioSource>();
//		_bgmPlayer.volume = Volume;
//		_bgmPlayer.priority = Priotity;
//		_bgmLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
//	}
//
//	public void PlayBgmByIndex(int index) {
//		if (index >= _bgm.Length)
//			return;
//
//		_bgmPlayer.Stop();
//		_bgmPlayer.clip = _bgm[index];
//		_bgmPlayer.loop = true;
//		_bgmPlayer.Play();
//
//	}
//
//	IEnumerator LerpFilterFrequency(float targetFrequency, float timeToLerp) {
//
//		float lastFrequency = _bgmLowPassFilter.cutoffFrequency;
//		float timer = 0;
//		while (timer < timeToLerp) {
//			_bgmLowPassFilter.cutoffFrequency = Mathf.Lerp(lastFrequency, targetFrequency, timer / timeToLerp);
//			timer += Time.deltaTime;
//			yield return null;
//		}
//
//		_bgmLowPassFilter.cutoffFrequency = targetFrequency;
//	}
//
//	public void ChangeLowPassFilterFrequency(float targetFrequency, float timeToChange) {
//		StopAllCoroutines();
//		StartCoroutine(LerpFilterFrequency(targetFrequency, timeToChange));
//	}
//
//
//	IEnumerator DramaticChange(float firstTransitionTime, float waitTime, float secondTransitionTime,
//		float firstTransitionFrequency, float secondTransitionFrequency) {
//
//		yield return StartCoroutine(LerpFilterFrequency(firstTransitionFrequency, firstTransitionTime));
//
//		yield return new WaitForSeconds(waitTime);
//
//		yield return StartCoroutine(LerpFilterFrequency(secondTransitionFrequency, secondTransitionTime));
//
//	}
//
//	public void SetBGMPitch(float pitch) {
//		_bgmPlayer.pitch = pitch;
//	}
//
//	public void SetBGMLowPassFilter(float value) {
//		_bgmLowPassFilter.cutoffFrequency = value;
//	}
//
//	public void DramaticFrequencyChange(float firstTransitionTime, float waitTime, float secondTransitionTime,
//		float firstTransitionFrequency, float secondTransitionFrequency) {
//		StartCoroutine(DramaticChange(firstTransitionTime, waitTime, secondTransitionTime, firstTransitionFrequency,
//			secondTransitionFrequency));
//	}
//}
