using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorProxy : Singleton<MonitorProxy> {

	public Animator Anima { get; private set; }
	public float ScrollBoundary;
	public float ScrollSpeed = 1;
	public float FlickeringAmount;
	[SerializeField ]private bool _shouldScroll;

	private Material _material;
	// Use this for initialization
	private void Awake() {
		_material = GetComponent<Renderer>().material;
		Anima = GetComponent<Animator>();
	}

	private void Update() {
		_material.SetFloat("_Flickering", Time.time % FlickeringAmount);
		
		if (!_shouldScroll)
			return;
		
		_material.SetFloat("_XOffSet", ScrollSpeed * Time.time % (ScrollBoundary * 2) - ScrollBoundary);
		
	}

	public void DoScroll(bool value) {
		_shouldScroll = value;
		if (!value) {
			_material.SetFloat("_XOffSet", 0);
		}
	}
}
