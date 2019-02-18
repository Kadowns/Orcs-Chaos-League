using System.Collections;
using UnityEngine;

namespace OCL {
    public class AudioChannel : MonoBehaviour {

        public float Volume {
            get { return m_audio.volume; }
            set { m_audio.volume = value; }
        }

        public float Pitch { 
            get { return m_audio.pitch; }
            set { m_audio.pitch = value; }
            
        }
        
        public bool Loop { 
            get { return m_audio.loop; }
            set { m_audio.loop = value; }            
        }

        public bool IsPlaying {
            get {
                return m_audio.isPlaying;
            }
        }

        public int Priority {
            get { return m_audio.priority; }
            set { m_audio.priority = value; }
        }

        public bool IsPrivate { get; set; }

        public AudioClip Clip {
            get { return m_audio.isPlaying ? m_audio.clip : null; }
        }
        
        private AudioSource m_audio;

        private Coroutine m_volumeRoutine;

        protected virtual void Awake() {
            m_audio = gameObject.AddComponent<AudioSource>();
        }

        public void Play(AudioClip clip) {
            m_audio.clip = clip;
            m_audio.Play();
        }
        
        public void SetVolumeSmooth(float volume, float time) {
            if (m_volumeRoutine != null) {
                StopCoroutine(m_volumeRoutine);
            }

            m_volumeRoutine = StartCoroutine(DoVolumeSmooth(volume, time));
        }
        
        public void Stop() {
            m_audio.Stop();
        }

        private IEnumerator DoVolumeSmooth(float volume, float time) {
            float lastVolume = Volume;
            float timer = 0;
            while (timer < time) {
                Volume = Mathf.Lerp(lastVolume, volume, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }

            Volume = volume;
        }        
    }
}