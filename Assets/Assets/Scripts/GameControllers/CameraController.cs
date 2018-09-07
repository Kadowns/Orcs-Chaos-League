using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {


	public float TargetZoomMultiplier = 4f;

	[SerializeField] private float _maxSize;
	[SerializeField] private float _minSize;
	[SerializeField] private float _moveSpeed;
	
	private Camera _camera;
	private CameraProjectionChange _projectionChange;
	private Animator _animator;
	private GameObject[] _orcs;
	private GameObject[] _spawners;
	private Vector3 _target;
	private Transform _defaultTarget;
	private bool _maxZoom, _transition, _externAgent;
	private int _gameState, _aliveOrcs;
	
	private void Awake() {
		_defaultTarget = transform;
		_animator = GetComponent<Animator>();
		_camera = GetComponentInChildren<Camera>();
		_projectionChange = GetComponentInChildren<CameraProjectionChange>();
		UpdatePlayers();
	}
	
	private void FixedUpdate() {
		
		if (_transition)
			return;

		_aliveOrcs = ActiveOrcs();
		
		UpdateVelocity();

		if (_externAgent)
			return;
		
		UpdateZoom();

	}

	private void UpdateVelocity() {
		float oscilation =  Mathf.Sin(Time.time * 0.5f);

		_target = GetTarget() + new Vector3(oscilation, 0, oscilation);
		
		transform.position = Vector3.Lerp(transform.position, _target, _moveSpeed * Time.deltaTime);
	
	}


	private void UpdateZoom() {


		float targetZoom;


		if (_aliveOrcs > 0 && !_maxZoom) {
			targetZoom = Mathf.Lerp(_minSize, _maxSize, GetTargetZoom() / 90);
		}
		else {
			targetZoom = _maxSize;
		}

		float oscilation = Mathf.Sin(Time.time * Mathf.Sin(Time.time * Mathf.Sin(Time.time * 0.1f) * 0.3f) * 0.05f) * 0.075f;
		
		if (_camera.orthographic) {
			_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom + (oscilation), 0.1f);
		}
		else {
			_camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition,
				-_camera.transform.forward * (targetZoom * TargetZoomMultiplier), 0.1f);
		}
	}


	private float GetTargetZoom() {
		float maxDist = 0;
		if (_aliveOrcs > 1) {
			for (int i = 0; i < _orcs.Length; i++) {
				if (!_orcs[i].activeInHierarchy) continue;
				for (int j = 0; j < _orcs.Length; j++) {
					if (j <= i || !_orcs[j].activeInHierarchy) continue;
					float dist = (_orcs[i].transform.position - _orcs[j].transform.position).sqrMagnitude;
					if (dist > maxDist) {
						maxDist = dist;
					}
				}
			}
		}
		else {
			for (int i = 0; i < _orcs.Length; i++) {
				if (!_orcs[i].activeInHierarchy) continue;			
				maxDist = (_defaultTarget.position - _orcs[i].transform.position).sqrMagnitude;
				break;
			}
		}
		
		return Mathf.Sqrt(maxDist);
	}


	private Vector3 GetTarget() {
		Vector3 target = Vector3.zero;


		switch (_aliveOrcs) {
			case 0:
				return _defaultTarget.position;
			case 1:
				foreach (var p in _orcs) {
					if (!p.activeInHierarchy) {
						continue;
					}

					target += p.transform.position;
					break;
				}

				var div = _aliveOrcs * 1.5f;
				
				target = new Vector3(target.x / div, target.y, target.z / div);
	
				break;
			default:
				foreach (var p in _orcs) {
					if (!p.activeInHierarchy) {
						continue;
					}

					target += p.transform.position;
				}
				
				var d = _aliveOrcs * 1.2f;
				
				target = new Vector3(target.x / d, target.y / _aliveOrcs, target.z / d);
				
				break;
		}

		target.Set(target.x, target.y - _defaultTarget.position.y, target.z);
		return target + _defaultTarget.position;
	}

	public void UpdateGameState(int state) {
		_gameState = state;
	}

	public void MaxZoom(bool value) {
		_maxZoom = value;
	}

	public void ChangePerspective() {
		_projectionChange.ChangeProjection = true;
		_transition = !_transition;

	}

	public void DoTransition(string transitionName) {
		_transition = true;
		StartAnimator();
		_animator.SetTrigger(transitionName);
	}	

	public void SceneCenter(Transform newTarget) {
		_defaultTarget = newTarget;
	}

	public void StopAnimator() {
		GameController.Instance.SetPlayersInput("Default", true);
		_animator.enabled = false;
		_transition = false;
	}

	public void ExternAgent(bool value) {
		_externAgent = value;
	}

	public bool InTransition() {
		return _transition;
	}

	public void StartAnimator() {
		GameController.Instance.SetPlayersInput("Default", false);
		_animator.enabled = true;
	}

	public int ActiveOrcs() {
		int sum = 0;
		foreach (var orc in _orcs) {
			if (orc.activeInHierarchy)
				sum++;
		}

		return sum;
	}
	
	public void UpdatePlayers() {		
		_orcs = GameObject.FindGameObjectsWithTag("Player");
	}

	public float GetMaxSize() {
		return _maxSize;
	}

	public float GetMinSize() {
		return _minSize;
	}
}
