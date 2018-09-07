using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour {


	public void Rematch() {
		GameController.Instance.Rematch();
	}

	public void GoToHub() {
		GameController.Instance.GoToHub();
	}
	
	
}
