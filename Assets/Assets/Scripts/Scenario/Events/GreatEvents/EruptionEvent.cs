using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreatEvent/Eruption")]
public class EruptionEvent : GreatEvent {

    public bool Eruption { get; private set; }

    protected override void OnExecute(ArenaState state) {
        
        Eruption = true;
        
        state.StartCoroutine(DoEruption(state));
    }

    protected  override void OnTerminate(ArenaState state) {
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
                int rand = Random.Range(0, state.LavaGeysers.Length);
                state.LavaGeysers[rand].Execute();
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }

            yield return new WaitForSeconds(3f);
        } while (Eruption);
    }
}
