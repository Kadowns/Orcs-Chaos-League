using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHUDProxy : Singleton<EventHUDProxy> {


	[SerializeField] private Image _eventIconImage;
	
	private Animator _animator;

	private void Awake() {
		_animator = GetComponent<Animator>();
	}

	public void ShowEventHUD(Sprite type) {
		_eventIconImage.sprite = type;
		_animator.SetTrigger("Show");
	}

	public void HideEventHUD() {
		_animator.SetTrigger("Hide");
	}
}
