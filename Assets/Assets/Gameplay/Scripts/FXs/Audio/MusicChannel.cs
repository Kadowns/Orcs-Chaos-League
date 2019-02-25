using System.Collections;
using UnityEngine;

namespace OCL {
    
    public class MusicChannel : AudioChannel {

        public float CutoffFrequency {
            get { return m_filter.cutoffFrequency; }
            set { m_filter.cutoffFrequency = value; }
        }

        private AudioLowPassFilter m_filter;

        private Coroutine m_cutoffRoutine;

        protected override void Awake() {
            base.Awake();
            m_filter = gameObject.AddComponent<AudioLowPassFilter>();
        }

        public void SetCutoffFrequency(float frequency, float time) {
            if (m_cutoffRoutine != null) {
                StopCoroutine(m_cutoffRoutine);
            }

            m_cutoffRoutine = StartCoroutine(DoCutoffFrequency(frequency, time));
        }

        public void SetFrequencyDramatic(float firstFrequency, float firstTime, float delayTime, float secondFrequency,
            float secondTime) {
            if (m_cutoffRoutine != null) {
                StopCoroutine(m_cutoffRoutine);
            }

            m_cutoffRoutine = StartCoroutine(DoDramaticFrequency(firstFrequency, firstTime, delayTime, secondFrequency, secondTime));
        }

        private IEnumerator DoDramaticFrequency(
            float firstFrequency,
            float firstTime,
            float delayTime,
            float secondFrequency,
            float secondTime) {
            
            yield return StartCoroutine(DoCutoffFrequency(firstFrequency, firstTime));
            yield return new WaitForSeconds(delayTime);
            yield return StartCoroutine(DoCutoffFrequency(secondFrequency, secondTime));
        }

        private IEnumerator DoCutoffFrequency(float frequency, float time) {
            float lastFrequency = CutoffFrequency;
            float timer = 0;
            while (timer < time) {
                CutoffFrequency = Mathf.Lerp(lastFrequency, frequency, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }

            CutoffFrequency = frequency;
        }

    }
}