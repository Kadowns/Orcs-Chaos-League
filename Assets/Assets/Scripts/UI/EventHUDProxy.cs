using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHUDProxy : Singleton<EventHUDProxy> {

	[System.Serializable]
	public class EventIconType {
		public GreatEvent.GreatEventType Type;
		public Sprite Sprite;
	}

	[SerializeField] private Image _eventIconImage;

	public List<EventIconType> EventIconTypes;
	
	private Animator _animator;

	private void Awake() {
		_animator = GetComponent<Animator>();
	}

	public void ShowEventHUD(GreatEvent.GreatEventType type) {
		_eventIconImage.sprite = EventIconTypes[(int) type].Sprite;
		_animator.SetTrigger("Show");
	}

	public void HideEventHUD() {
		_animator.SetTrigger("Hide");
	}
}
