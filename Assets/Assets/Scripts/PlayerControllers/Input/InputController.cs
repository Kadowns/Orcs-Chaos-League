using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerControllers.Input;
using UnityEngine;

public class InputController : MonoBehaviour {


	public InputSource Source;

	public int PlayerNumber { get; private set; }

	public Vector2 Axis { get; set; }

	public bool CenterButtonPresssed { get; set; }

	public bool ActionDownButton { get; set; }
	public bool ActionUpButton { get; set; }
	public bool ActionLeftButton { get; set; }
	public bool ActionRightButton { get; set; }

	public bool ActionDownButtonPressed { get; set; }
	public bool ActionUpButtonPressed { get; set; }
	public bool ActionLeftButtonPressed { get; set; }
	public bool ActionRightButtonPressed { get; set; }

	public bool ActionDownButtonReleased { get; set; }
	public bool ActionUpButtonReleased { get; set; }
	public bool ActionLeftButtonReleased { get; set; }
	public bool ActionRightButtonReleased { get; set; }

	public bool DPadUp { get; set; }
	public bool DPadDown { get; set; }
	public bool DPadLeft { get; set; }
	public bool DPadRight { get; set; }

	public bool DPadUpPressed { get; set; }
	public bool DPadDownPressed { get; set; }
	public bool DPadLeftPressed { get; set; }
	public bool DPadRightPressed { get; set; }

	public bool DPadUpReleased { get; set; }
	public bool DPadDownReleased { get; set; }
	public bool DPadLeftReleased { get; set; }
	public bool DPadRightReleased { get; set; }

	private void Awake() {
		PlayerNumber = GetComponent<PlayerController>().PlayerNumber;
	}

	private void Update() {
		Source.Tick(this);
	}
}
