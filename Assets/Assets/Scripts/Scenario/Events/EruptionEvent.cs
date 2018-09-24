using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/Eruption")]
public class EruptionEvent : GreatEvent {

    public bool Eruption { get; private set; }

    public override void Setup(ArenaState state) {
        
    }

    public override void Execute(ArenaState state) {
        EventHUDProxy.Instance.ShowEventHUD(EventType);
        Eruption = true;
        var fx = ScreenEffects.Instance;
        fx.ScreenShake(1.5f, 5f);
        fx.BlurForSeconds(1.5f, 20f, new Color(0.85f, 0.85f, 0.85f), new Color(1f, 0.85f, 0.85f));
        var music = MusicController.Instance;
        music.PlayBgmByIndex(1);
        music.SetBGMPitch(1.075f);
        music.DramaticFrequencyChange(0.1f, 1.5f, 0.5f, 200f, 22000f);
        state.StartCoroutine(DoEruption(state));
    }

    public override void Terminate(ArenaState state) {
        EventHUDProxy.Instance.HideEventHUD();
        ScreenEffects.Instance.Blur(0, Color.white);
        MusicController.Instance.SetBGMPitch(1f);
        Eruption = false;
        foreach (var platforms in state.Plataforms) {
            platforms.Raise();
        }
    }

    private IEnumerator DoEruption(ArenaState state) {

        state.StartCoroutine(EruptionExplosions(state));
        int rand = Random.Range(0, state.Plataforms.Count);
        int sunkPlataforms = 0;
        bool[] sunk = new bool[state.Plataforms.Count];
        do {

            int plataformsToSink = state.Plataforms.Count - (int)(state.Plataforms.Count * 0.5f);
			Debug.Log(plataformsToSink);
            if (sunkPlataforms < plataformsToSink) {
                state.Plataforms[rand].Lower();		
                sunk[rand] = true;
                sunkPlataforms++;
                rand = rand + 1 >= state.Plataforms.Count ? 0 : rand + 1;
                yield return new WaitForSeconds(0.5f);
            }
            else {

                int i = rand;
                do {

                    if (sunk[i]) {
                        state.Plataforms[i].Raise();
                        sunkPlataforms--;
                        sunk[i] = false;
                        yield return new WaitForSeconds(2f);
                        break;
                    }

                    i = i + 1 >= state.Plataforms.Count ? 0 : i + 1;
                } while (true);
            }

        } while (Eruption);
    }

    private IEnumerator EruptionExplosions(ArenaState state) {
        do {
            int explosions = Random.Range(5, 10);
            for (int i = 0; i < explosions; i++) {
                int rand = Random.Range(0, state.EventSpawners.Length);
                state.EventSpawners[rand].Execute();
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }

            yield return new WaitForSeconds(3f);
        } while (Eruption);
    }
}
