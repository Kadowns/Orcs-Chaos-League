using UnityEngine;

namespace OCL {

    public class AudioPlayer : MonoBehaviour {
        
        [SerializeField]
        private AudioClip m_clip;
        
        [SerializeField]
        private bool m_randomPitch;

        public bool PlayOnAwake;

        public byte Priority = 128;

        [Range(0, 1)]
        public float Volume = 1;

        public bool Loop = false;
        
        
        private void Start() {
            if (PlayOnAwake)
                Play();
        }

        public void Play() {
            AudioController.Instance.Play(
                m_clip,
                AudioController.SoundType.ExclusiveSoundEffect,
                false,
                m_randomPitch ? Random.Range(0.8f, 1.2f) : 1,
                Priority,
                Volume,
                Loop
            );
        }
    }
}