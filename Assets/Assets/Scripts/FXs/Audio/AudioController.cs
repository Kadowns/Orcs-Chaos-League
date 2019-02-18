using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace OCL {
	
	public class AudioController : Singleton<AudioController> {

		[SerializeField] 
		private int _exclusiveSoundEffects = 1, _globalSoundEffects = 1;
		
		[Range(0, 1)]
		public float MusicVolume, SoundEffectVolume, NarrationVolume, MasterVolume;
		
		public enum SoundType {
			Music,
			ExclusiveSoundEffect,
			GlobalSoundEffect,
			Narration, 
			All
		}

		private MusicChannel m_musicChannel;
		private AudioChannel m_narration;

		private List<AudioChannel> m_exclusiveSoundEffectChannels = new List<AudioChannel>();
		private List<AudioChannel> m_globalSoundEffectChannels = new List<AudioChannel>();

#if UNITY_EDITOR
		private void OnValidate() {
			if (_exclusiveSoundEffects < 1)
				_exclusiveSoundEffects = 1;

			if (_globalSoundEffects < 1)
				_globalSoundEffects = 1;
			
			
		}
#endif

		private void Awake() {

			m_musicChannel = gameObject.AddChildWithComponent<MusicChannel>("MusicChannel");
			m_narration = gameObject.AddChildWithComponent<AudioChannel>("NarrationChannel");
			
			for (int i = 0; i < _exclusiveSoundEffects; i++) {
				m_exclusiveSoundEffectChannels.Add(gameObject.AddChildWithComponent<AudioChannel>("SoundEffectChannel" + i));
			}

			for (int i = 0; i < _globalSoundEffects; i++) {
				m_globalSoundEffectChannels.Add(gameObject.AddComponent<AudioChannel>());
			}
		}

		public void ChangeCutoffFrequency(
			float firstFrequency, 
			float firstTime, 
			float delay, 
			float secondFrequency,
			float secondTime) {			
			m_musicChannel.SetFrequencyDramatic(firstFrequency, firstTime, delay, secondFrequency, secondTime);			
		}

		public void ChangeCutoffFrequency(float frequency, float time) {
			m_musicChannel.SetCutoffFrequency(frequency, time);
		}

		public void SetCutoffFrequency(float frequency) {
			m_musicChannel.CutoffFrequency = frequency;
		}

		public void SetPitch(float pitch, SoundType type) {
			switch (type) {
				case SoundType.Music:
					m_musicChannel.Pitch = pitch;
					break;
				case SoundType.ExclusiveSoundEffect:
					m_exclusiveSoundEffectChannels.ForEach(c => c.Pitch = pitch);
					break;
				case SoundType.Narration:
					m_narration.Pitch = pitch;
					break;
				case SoundType.GlobalSoundEffect:
					m_globalSoundEffectChannels.ForEach(c => c.Pitch = pitch);
					break;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		public void SetVolume(float volume, SoundType type) {
			
			switch (type) {
				case SoundType.Music:					
					m_musicChannel.Volume = (MusicVolume = volume) * MasterVolume;
					break;
				case SoundType.ExclusiveSoundEffect:
					m_exclusiveSoundEffectChannels.ForEach(c => c.Volume = (SoundEffectVolume = volume) * MasterVolume);						
					break;
				case SoundType.Narration:					
					m_narration.Pitch = (NarrationVolume = volume) * MasterVolume;
					break;
				case SoundType.GlobalSoundEffect:
					m_globalSoundEffectChannels.ForEach(c => c.Volume = (SoundEffectVolume = volume) * MasterVolume);
					break;
				case SoundType.All:				
					m_musicChannel.Volume = (MusicVolume = volume) * MasterVolume;
					SoundEffectVolume = volume;
					m_exclusiveSoundEffectChannels.ForEach(c => c.Volume = SoundEffectVolume * MasterVolume);
					m_globalSoundEffectChannels.ForEach(c => c.Volume = SoundEffectVolume * MasterVolume);
					m_narration.Pitch = (NarrationVolume = volume) * MasterVolume;
					break;					
			}
		}

		public void Stop(AudioClip clip, SoundType type) {
			switch (type) {
				case SoundType.Music:
					m_musicChannel.Stop();
					break;
				case SoundType.ExclusiveSoundEffect:
					m_exclusiveSoundEffectChannels.First(c => c.Clip == clip).Stop();
					break;
				case SoundType.GlobalSoundEffect:
					m_globalSoundEffectChannels.First(c => c.Clip == clip).Stop();
					break;
				case SoundType.Narration:
					m_narration.Stop();
					break;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}
		
		
		public AudioChannel Play(AudioClip clip, SoundType type, bool isPrivate = false, float pitch = 1, int priority = 128, float volume = 1f, bool loop = false) {
			
			if (clip == null) {
				Debug.LogWarning("Null clip on Play!");
				return null;
			}

			//menor prioridade tem na verdade maior prioridade na unity (wtf)
			priority = 255 - priority;
			volume *= MasterVolume;
			
			switch (type) {
				
				case SoundType.Music:
					m_musicChannel.Volume = volume * MusicVolume;
					m_musicChannel.Pitch = pitch;
					m_musicChannel.Loop = true;
					m_musicChannel.Play(clip);
					m_musicChannel.IsPrivate = isPrivate;
					m_musicChannel.Priority = priority;
					return m_musicChannel;

				case SoundType.ExclusiveSoundEffect: {
					AudioChannel channel = m_exclusiveSoundEffectChannels.FirstOrDefault(c => !c.IsPlaying && !c.IsPrivate);
					if (channel == null) {
						channel = m_exclusiveSoundEffectChannels.FirstOrDefault(c => c.Priority > priority && !c.IsPrivate);
						if (channel == null) {
							return null;
						}							
					}
			
					channel.gameObject.SetActive(true);
					channel.Volume = volume * SoundEffectVolume;
					channel.Pitch = pitch;
					channel.Loop = loop;
					channel.IsPrivate = isPrivate;
					channel.Priority = priority;
					channel.Play(clip);

					return channel;
				}				
				case SoundType.Narration:
					m_narration.Volume = volume * NarrationVolume;
					m_narration.Pitch = pitch;
					m_narration.Loop = loop;
					m_narration.Play(clip);
					m_narration.IsPrivate = isPrivate;
					m_narration.Priority = priority;
					return m_narration;
				case SoundType.GlobalSoundEffect: {

					AudioChannel channel = m_globalSoundEffectChannels.FirstOrDefault(c => !c.IsPlaying && !c.IsPrivate);
					if (channel == null) {
						channel = m_globalSoundEffectChannels.FirstOrDefault(c => c.Priority > priority && !c.IsPrivate);
						if (channel == null) {
							return null;
						}
					}
					
					channel.Volume = volume * SoundEffectVolume;
					channel.Pitch = pitch;
					channel.Loop = loop;
					channel.Play(clip);
					channel.Priority = priority;
					channel.IsPrivate = isPrivate;

					return channel;
				}
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}
	}
}