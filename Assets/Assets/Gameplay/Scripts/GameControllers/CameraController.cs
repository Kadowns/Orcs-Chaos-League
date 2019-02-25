using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CameraController : Singleton<CameraController> {


	public float TargetZoomMultiplier = 4f;

	[SerializeField] private float _maxSize;
	[SerializeField] private float _minSize;
	[SerializeField] private float _moveSpeed;

	public event Action OnIntroFinished;
	public event Action OnIntroStarted;
	
	private Camera _camera;
	private Animator _animator;
	private GameObject[] _orcs;
	private BoxEntityState[] _spawners = new BoxEntityState[0];
	private Vector3 _target;
	private bool  _transition = true, _externAgent;
	private int _aliveOrcs, _activeSpawners;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_camera = GetComponentInChildren<Camera>();
		UpdatePlayers();
	}
	

	
	private void FixedUpdate() {
		
		if (_transition)
			return;

		_aliveOrcs = ActiveOrcs();
		_activeSpawners = ActiveSpawners();
		
		UpdateVelocity();

		if (_externAgent)
			return;
		
		UpdateZoom();

	}

	public void CameraIntroFinished() {
		if (OnIntroFinished != null) {
			OnIntroFinished();
		}
		var objs = GameObject.FindGameObjectsWithTag("Spawner").ToList();
		objs.ForEach(obj => { _spawners.Add(obj.GetComponent<BoxEntityState>()); });
	}

	public void CameraIntroStarted() {
		if (OnIntroStarted != null) {
			OnIntroStarted();
		}
	}

	private void UpdateVelocity() {
		float oscilation =  Mathf.Sin(Time.time * 0.5f);

		_target = GetTarget() + new Vector3(oscilation, 0, oscilation);
		
		transform.position = Vector3.Lerp(transform.position, _target, _moveSpeed * Time.deltaTime);
	
	}


	private void UpdateZoom() {


		float targetZoom;


		if (_aliveOrcs > 0) {
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
				if (!_orcs[i].activeInHierarchy)
					continue;
				for (int j = 0; j < _orcs.Length; j++) {
					if (j <= i || !_orcs[j].activeInHierarchy)
						continue;
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
				maxDist = _orcs[i].transform.position.sqrMagnitude;
				break;
			}
		}
		
		return Mathf.Sqrt(maxDist);
	}


	private Vector3 GetTarget() {
		Vector3 target = Vector3.zero;

		if (_aliveOrcs == 0 && _activeSpawners == 0) {
			return target;
		}
		
		foreach (var p in _orcs) {
			if (!p.activeInHierarchy) {
				continue;
			}
			target += p.transform.position;
		}

		foreach (var s in _spawners) {
			if (s.CanSpawn) {
				target += new Vector3(s.transform.position.x, 0, s.transform.position.z);
			}
		}

		var d = (_aliveOrcs + _activeSpawners) * 1.2f;
		target /= d;
		return target;
		
//		switch (_aliveOrcs) {
//			case 0:
//				return target;
//			case 1:
//				foreach (var p in _orcs) {
//					if (!p.activeInHierarchy) {
//						continue;
//					}
//
//					target += p.transform.position;
//					break;
//				}
//
//				var div = _aliveOrcs * 1.5f;
//				
//				target = new Vector3(target.x / div, target.y, target.z / div);
//	
//				break;
//			default:
//				foreach (var p in _orcs) {
//					if (!p.activeInHierarchy) {
//						continue;
//					}
//
//					target += p.transform.position;
//				}
//				
//				var d = _aliveOrcs * 1.2f;
//				
//				target = new Vector3(target.x / d, target.y / _aliveOrcs, target.z / d);
//				
//				break;
//		}
//
//		target.Set(target.x, target.y, target.z);
//		return target;
	}

	public void DoTransition(string transitionName) {
		_transition = true;
		StartAnimator();
		_animator.SetTrigger(transitionName);
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

	public int ActiveSpawners() {
		int sum = 0;
		foreach (var spawner in _spawners) {
			if (spawner.CanSpawn)
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
