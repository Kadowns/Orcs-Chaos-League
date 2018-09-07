using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSource : MonoBehaviour {

	public float AxisX { get; protected set; }
	public float AxisY { get; protected set; }
	
	public bool CenterButtonPresssed { get; protected set; }
	
	public bool ActionDownButton { get; protected set; }
	public bool ActionUpButton { get; protected set; }
	public bool ActionLeftButton { get; protected set; }
	public bool ActionRightButton { get; protected set; }
	
	public bool ActionDownButtonPressed { get; protected set; }
	public bool ActionUpButtonPressed { get; protected set; }
	public bool ActionLeftButtonPressed { get; protected set; }
	public bool ActionRightButtonPressed { get; protected set; }
	
	public bool ActionDownButtonReleased { get; protected set; }
	public bool ActionUpButtonReleased { get; protected set; }
	public bool ActionLeftButtonReleased { get; protected set; }
	public bool ActionRightButtonReleased { get; protected set; }
	
	public bool DPadUp { get; protected set; }
	public bool DPadDown { get; protected set; }
	public bool DPadLeft { get; protected set; }
	public bool DPadRight { get; protected set; }
	
	public bool DPadUpPressed { get; protected set; }
	public bool DPadDownPressed { get; protected set; }
	public bool DPadLeftPressed { get; protected set; }
	public bool DPadRightPressed { get; protected set; }
	
	public bool DPadUpReleased { get; protected set; }
	public bool DPadDownReleased { get; protected set; }
	public bool DPadLeftReleased { get; protected set; }
	public bool DPadRightReleased { get; protected set; }
}
