using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowCaster : MonoBehaviour {


	[SerializeField] private GameObject _shadowPrefab;
	
	private GameObject _shadow;

	private Transform _tr;

	private Renderer _renderer;

	[SerializeField] private bool _castLine, _castShadow;

    public float DistanceThreshold = 10, MinimumSize = 1.5f;

	private LineRenderer _line;

	
	private void Start() {

		if (_castShadow)
			CreateShadow();
		if (_castLine)
			CreateLine();
		
	}

	private void CreateShadow() {
		_shadow = Instantiate(_shadowPrefab, transform.position, Quaternion.Euler(90, 0, 0), transform);
		_renderer = _shadow.GetComponent<Renderer>();
		_tr = _shadow.GetComponent<Transform>();
	}

	private void CreateLine() {
		_line = gameObject.AddComponent<LineRenderer>();
		_line.shadowCastingMode = ShadowCastingMode.Off;
		_line.material = new Material(Shader.Find("UI/Default"));
		var pp = GetComponentInParent<PlayerPointer>();
		_line.material.color = pp != null ? pp.GetPlayerColor() : Color.black;
		_line.startWidth = 0.2f;
		_line.positionCount = 2;
	}

	public void SetLineColor(Color c) {
		_line.material.color = c;
	}

	private void LateUpdate() {
		float dist = DistanceToGround();

		if (_tr != null) {
			if (_castShadow) {
				_tr.rotation = Quaternion.Euler(90, 0, 0);
				_tr.position = new Vector3(transform.position.x, dist + transform.position.y, transform.position.z);
				if (MinimumSize + dist / DistanceThreshold > 0)
					_tr.localScale = new Vector3(MinimumSize + dist / DistanceThreshold, MinimumSize + dist / DistanceThreshold, 1);

				else {
					_tr.localScale = Vector3.forward;
				}

				float alpha = Mathf.Lerp(1f, 0.6f, -dist / 10);
				Color c = _renderer.material.color;
				_renderer.material.color = new Color(c.r, c.g, c.b, alpha);
			}
		}

		if (_line != null) {
			if (_castLine) {
				_line.enabled = true;
				_line.SetPosition(0, transform.position);
				_line.SetPosition(1, new Vector3(transform.position.x, dist + transform.position.y, transform.position.z));
			} else {
				_line.enabled = false;
			}	
		}
		
	}

	public void SetLineEnabled(bool value) {
		_castLine = value;
	}
	
	private float DistanceToGround() {
		float dist = 0;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f)) {
			dist = hit.point.y - transform.position.y;
		}
		return dist + 0.1f;
	}
}
