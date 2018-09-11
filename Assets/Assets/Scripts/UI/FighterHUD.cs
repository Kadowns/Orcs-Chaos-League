using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHUD : MonoBehaviour {

	[SerializeField] private Text _damageText;
    [SerializeField] private Animator _lifeanim, _lifeanim2, _lifeanim3, _head;

	public PlayerController Controller;
   
    private int danoAtual, LifeCount = 0;
    private bool timerChange, shit2;

	private void Start() {
		Controller.ScoreEvent += UpdateDamage;
	}
	

    public void UpdateLife(int value) {
	 
        switch (value)
        {
            case 2:
                _lifeanim.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 2);

                break;
            case 1:
                _lifeanim2.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 1);
                LifeCount = 0;

                break;
            case 0:
                _lifeanim3.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 0);
                LifeCount++;

                break;

        }

        if (LifeCount >= 2 || value == -1 && LifeCount >= 1)
        {
            _head.SetInteger("StateChange", 4);
            //shit = 0;
        }

        ResetVariables(shit2);
       
    }

    public void UpdateDamage(string text) {
		_damageText.transform.localScale = Vector3.one;
		_damageText.text = text;
	}
	
	public void UpdateDamage(int damage) {
		_damageText.transform.localScale = Vector3.one;
		_damageText.color = Color.Lerp(Color.white, Color.red, (float)damage / 100);
		
		StartCoroutine(ImpactScale(_damageText, damage, 0.25f, 2f));
    }

	private IEnumerator ImpactScale(Text text, int targetDamage, float timeToScale, float scale) {

		float scaleTimer = 0;
		while (scaleTimer < timeToScale) {
			scaleTimer += Time.deltaTime;
			text.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, scaleTimer / timeToScale);
            danoAtual = (int)Mathf.Lerp(danoAtual, targetDamage, scaleTimer / timeToScale);
            text.text = "" + danoAtual;
			yield return null;
		}
		text.transform.localScale = Vector3.one;
	}

    public void ResetVariables(bool retorno)
    {
        if(retorno == true)
        {
            shit2 = retorno;
            LifeCount = 0;
            Debug.Log(retorno);
        }
    }
}
