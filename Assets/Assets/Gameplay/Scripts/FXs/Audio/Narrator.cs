using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OCL {

	public class Narrator : Singleton<Narrator> {

		[Range(0, 1)]
		public float PlaydownVolume;
		
		[SerializeField]
		private float m_delayTime = 5;
		
		[SerializeField] private List<AudioClip> m_onDeathNarration;
		
		private List<PlayerController> m_players;

		private float m_timer, m_cachedMusicVolume, m_cachedSfxVolume;

		private Coroutine m_speakRoutine;
		
		private void Awake() {
			m_players = GameController.Instance.PlayerControllers.ToList();
			m_players.ForEach(p => p.OnDeath += OnDeath);
		}

		private void OnDeath(int killerNumber) {
			if (m_timer > Time.time)
				return;

			m_timer = Time.time + m_delayTime;

			bool cacheVolumes = true;
			if (m_speakRoutine != null) {
				StopCoroutine(m_speakRoutine);
				cacheVolumes = false;
			}
			var clip = m_onDeathNarration[Random.Range(0, m_onDeathNarration.Count - 1)];
			Debug.Log(clip.name);
			m_speakRoutine = StartCoroutine(			
				Speak(clip, cacheVolumes)
			);
		}


		private IEnumerator Speak(AudioClip clip, bool cacheVolumes) {

			if (cacheVolumes) {			
				m_cachedMusicVolume = AudioController.Instance.MusicVolume;
				m_cachedSfxVolume = AudioController.Instance.SoundEffectVolume;	
			}
			
			float timer = 0;
			
			do {

				AudioController.Instance.SetVolume(Mathf.Lerp(m_cachedMusicVolume, PlaydownVolume, timer / 0.4f),
					AudioController.SoundType.Music);
				AudioController.Instance.SetVolume(Mathf.Lerp(m_cachedSfxVolume, PlaydownVolume, timer / 0.4f),
					AudioController.SoundType.GlobalSoundEffect);
				AudioController.Instance.SetVolume(Mathf.Lerp(m_cachedSfxVolume, PlaydownVolume, timer / 0.4f),
					AudioController.SoundType.ExclusiveSoundEffect);

				yield return null;
			} while ((timer += Time.deltaTime) < 0.4f);

			AudioController.Instance.SetVolume(PlaydownVolume, AudioController.SoundType.Music);
			AudioController.Instance.SetVolume(PlaydownVolume, AudioController.SoundType.GlobalSoundEffect);
			AudioController.Instance.SetVolume(PlaydownVolume, AudioController.SoundType.ExclusiveSoundEffect);
			
			yield return new WaitForSeconds(0.4f);

			AudioController.Instance.Play(clip, AudioController.SoundType.Narration);
			
			yield return new WaitForSeconds(clip.length + 1);

			timer = 0;

			do {

				AudioController.Instance.SetVolume(Mathf.Lerp(PlaydownVolume, m_cachedMusicVolume, timer / 0.8f),
					AudioController.SoundType.Music);
				AudioController.Instance.SetVolume(Mathf.Lerp(PlaydownVolume, m_cachedSfxVolume, timer / 0.8f),
					AudioController.SoundType.GlobalSoundEffect);
				AudioController.Instance.SetVolume(Mathf.Lerp(PlaydownVolume, m_cachedSfxVolume, timer / 0.8f),
					AudioController.SoundType.ExclusiveSoundEffect);
				
				yield return null;
			} while ((timer += Time.deltaTime) < 0.8f);

			AudioController.Instance.SetVolume(m_cachedMusicVolume, AudioController.SoundType.Music);
			AudioController.Instance.SetVolume(m_cachedSfxVolume, AudioController.SoundType.GlobalSoundEffect);
			AudioController.Instance.SetVolume(m_cachedSfxVolume, AudioController.SoundType.ExclusiveSoundEffect);
		}
	}
}