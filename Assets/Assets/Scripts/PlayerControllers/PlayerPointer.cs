using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerPointer : MonoBehaviour {

	[SerializeField] private Color _playerColor;
	[SerializeField] private Vector3 _offSet;
	[SerializeField] private GameObject _pointer;
	[SerializeField] private Sprite[] _sprites;
	private SpriteRenderer _renderer;
	private Transform _target; 
	
	private void Awake() {

		_pointer = Instantiate(_pointer, transform.position, Quaternion.identity, transform);
		_renderer = _pointer.GetComponent<SpriteRenderer>();
		_renderer.sprite = _sprites[GetComponent<PlayerController>().PlayerNumber];
	}

	private void LateUpdate () {
		
		if (_target != null) {			
			_pointer.transform.rotation = Quaternion.identity;
			_pointer.transform.position = _target.position + _offSet;
			var size = Camera.main.orthographicSize / (CameraController.Instance.GetMaxSize() * 0.5f);
			_pointer.transform.localScale = new Vector3(size, size, 1);
		}
		
	}

	public Color GetPlayerColor() {
		return _playerColor;
	}

	public void SetTarget(Transform target) {
		if (target == null) {
			_renderer.enabled = false;
			return;
		}

		_renderer.enabled = true;
		_target = target;
	}

}
