using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHUD : MonoBehaviour {

	[SerializeField] private Text _scoreText;
    [SerializeField] private Text _killCountText;

	public PlayerController Controller;
   
    private int _actualDamage;

    private void OnEnable() {
        Controller.OnDamage += UpdateOnDamage;
        Controller.OnDeath += UpdateLife;
        Controller.OnSkullCollected += UpdateKills;
    }

    private void OnDisable() {
        Controller.OnDamage -= UpdateOnDamage;
        Controller.OnDeath -= UpdateLife;
        Controller.OnSkullCollected -= UpdateKills;
    }    

    public void UpdateLife(int value) {
        return;
    }

    public void UpdateKills(int count) {
        if (gameObject.activeInHierarchy)
            StartCoroutine(ImpactScale(_killCountText, "x", count, 0.1f, 2f));
    }
    
	
	public void UpdateOnDamage() {
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
            _actualDamage = (int)Mathf.Lerp(_actualDamage, targetScore, scaleTimer / timeToScale);
            text.text = defaultText + _actualDamage;
			yield return null;
		}
		text.transform.localScale = Vector3.one;
	}

    public void ResetToDefault() {
        _scoreText.text = "" + 0;
        _killCountText.text = "x" + 0;
    }
}
