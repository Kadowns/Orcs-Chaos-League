using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHUD : MonoBehaviour {

	[SerializeField] private Text _scoreText;
    [SerializeField] private Animator _head;

	public PlayerController Controller;
   
    private int danoAtual, LifeCount = 0;
    private bool timerChange, shit2;

    private void OnEnable()
    {
        Controller.ScoreEvent += UpdateScore;
        Controller.DeathEvent += UpdateLife;
    }

    private void OnDisable()
    {
        Controller.ScoreEvent -= UpdateScore;
        Controller.DeathEvent -= UpdateLife;
    }


    public void UpdateLife(int value) {
	 
        switch (value)
        {
            case 2:
              //  _lifeanim.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 2);

                break;
            case 1:
            //    _lifeanim2.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 1);
                LifeCount = 0;

                break;
            case 0:
          //      _lifeanim3.SetInteger("StateChange", 2);
                _head.SetInteger("StateChange", 0);
                LifeCount++;

                break;

        }

        if (LifeCount >= 2 || value == -1 && LifeCount >= 1)
        {
            _head.SetInteger("StateChange", 4);
        }

        ResetVariables(shit2);
       
    }

	
	public void UpdateScore() {
        var score = Controller.Score;
		_scoreText.transform.localScale = Vector3.one;
		_scoreText.color = Color.Lerp(Color.white, Color.green, (float)score / 999);
        if (gameObject.activeInHierarchy)
            StartCoroutine(ImpactScale(_scoreText, score, 0.25f, 2f));
    }

	private IEnumerator ImpactScale(Text text, int targetScore, float timeToScale, float scale) {

		float scaleTimer = 0;
		while (scaleTimer < timeToScale) {
			scaleTimer += Time.deltaTime;
			text.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, scaleTimer / timeToScale);
            danoAtual = (int)Mathf.Lerp(danoAtual, targetScore, scaleTimer / timeToScale);
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
        }
    }
}
