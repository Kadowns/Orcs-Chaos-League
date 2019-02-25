using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour {

	public bool ActiveAtStart;
	public GameObject[] Buttons;


	private void Start() {
		gameObject.SetActive(ActiveAtStart);
	}

	public void Enable(bool value) {
		gameObject.SetActive(value);
	}
}
