using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EruptionEvent : GreatEvent {

    public List<PlataformBehaviour> Plataforms;

    public bool Eruption { get; private set; }

    private void Start() {
        Eruption = false;
    }
    
    public override void Execute() {
        StartCoroutine(DoEruption());
    }

    public override void Terminate() {
        Eruption = false;
        StartCoroutine(RaisePlataforms());
    }
    
    private IEnumerator RaisePlataforms() {
        yield return null;
        foreach (var platforms in Plataforms) {
            platforms.Raise();
        }
    }

    
    protected IEnumerator DoEruption() {

      //  controller.StartCoroutine(SuddenDeathExplosions(controller, state));
        int rand = Random.Range(0, Plataforms.Count);
        int sunkPlataforms = 0;
        bool[] sunk = new bool[Plataforms.Count];
        do {

            int plataformsToSink = Plataforms.Count - (int)(Plataforms.Count * 0.5f);
			
            if (sunkPlataforms < plataformsToSink) {
                Plataforms[rand].Lower();		
                sunk[rand] = true;
                sunkPlataforms++;
                rand = rand + 1 >= Plataforms.Count ? 0 : rand + 1;
                yield return new WaitForSeconds(0.5f);
            }
            else {

                int i = rand;
                do {

                    if (sunk[i]) {
                        Plataforms[i].Raise();
                        sunkPlataforms--;
                        sunk[i] = false;
                        yield return new WaitForSeconds(2f);
                        break;
                    }

                    i = i + 1 >= Plataforms.Count ? 0 : i + 1;
                } while (true);
            }

        } while (Eruption);
    }
}
