using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHUD : MonoBehaviour {

	[SerializeField] private Text _scoreText;
    [SerializeField] private Text _killCountText;
    [SerializeField] private Animator _head;

	public PlayerController Controller;
   
    private int danoAtual, LifeCount = 0;
    private bool timerChange, shit2;

    private void OnEnable() {
        Controller.ScoreEvent += UpdateScore;
        Controller.DeathEvent += UpdateLife;
        Controller.KilledEvent += UpdateKills;
    }

    private void OnDisable() {
        Controller.ScoreEvent -= UpdateScore;
        Controller.DeathEvent -= UpdateLife;
        Controller.KilledEvent -= UpdateKills;
    }    

    public void UpdateLife(int value) {
        return;
    }

    public void UpdateKills(int count) {
        if (gameObject.activeInHierarchy)
            StartCoroutine(ImpactScale(_killCountText, "x", count, 0.1f, 2f));
    }
    
	
	public void UpdateScore() {
        var score = Controller.OrcDamage;
		_scoreText.transform.localScale = Vector3.one;
		_scoreText.color = Color.Lerp(Color.white, Color.red, (float)score / 100);
        if (gameObject.activeInHierarchy)
            StartCoroutine(ImpactScale(_scoreText, "", score, 0.25f, 2f));
    }

	private IEnumerator ImpactScale(Text text, string defaultText, int targetScore, float timeToScale, float scale) {

		float scaleTimer = 0;
		while (scaleTimer < timeToScale) {
			scaleTimer += Time.deltaTime;
			text.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, scaleTimer / timeToScale);
            danoAtual = (int)Mathf.Lerp(danoAtual, targetScore, scaleTimer / timeToScale);
            text.text = defaultText + danoAtual;
			yield return null;
		}
		text.transform.localScale = Vector3.one;
	}

    public void ResetToDefault() {
        _scoreText.text = "" + 0;
        _killCountText.text = "x" + 0;
    }
}
