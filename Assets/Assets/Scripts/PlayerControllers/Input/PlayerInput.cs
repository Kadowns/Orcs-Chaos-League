using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerControllers.Input;
using Rewired;
using UnityEngine;
using UnityScript.Steps;

[CreateAssetMenu(menuName = "InputSource/PlayerInput")]
public class PlayerInput : InputSource {
	

	public override void Tick(InputController input) {

		var memory = PlayerBrains.BrainMemory[input.PlayerNumber].Player;
		
		input.CenterButtonPresssed = memory.GetButtonDown("Pause");
		
		input.ActionDownButton = memory.GetButton("Jump");
		input.ActionDownButtonPressed = memory.GetButtonDown("Jump");
		input.ActionDownButtonReleased = memory.GetButtonUp("Jump");
			
		input.ActionUpButton = memory.GetButton("Parry");
		input.ActionUpButtonPressed = memory.GetButtonDown("Parry");
		input.ActionUpButtonReleased = memory.GetButtonUp("Parry");
		
		input.ActionLeftButton = memory.GetButton("Attack");
		input.ActionLeftButtonPressed = memory.GetButtonDown("Attack");
		input.ActionLeftButtonReleased = memory.GetButtonUp("Attack");
		
		input.ActionRightButton = memory.GetButton("Dash");
		input.ActionRightButtonPressed = memory.GetButtonDown("Dash");
		input.ActionRightButtonReleased = memory.GetButtonUp("Dash");
		
		input.DPadUp = memory.GetButton("Taunt1");
		input.DPadUpPressed = memory.GetButtonDown("Taunt1");
		input.DPadUpReleased = memory.GetButtonUp("Taunt1");

		input.Axis = new Vector2(memory.GetAxisRaw("Horizontal"), memory.GetAxisRaw("Vertical"));
	}	
}
