using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerInput : InputSource {
	
	[HideInInspector] public Player Player;

	private void Update() {

		if (Player == null)
			return;
		
		CenterButtonPresssed = Player.GetButtonDown("Pause");
		
		ActionDownButton = Player.GetButton("Jump");
		ActionDownButtonPressed = Player.GetButtonDown("Jump");
		ActionDownButtonReleased = Player.GetButtonUp("Jump");
			
		ActionUpButton = Player.GetButton("Parry");
		ActionUpButtonPressed = Player.GetButtonDown("Parry");
		ActionUpButtonReleased = Player.GetButtonUp("Parry");
		
		ActionLeftButton = Player.GetButton("Attack");
		ActionLeftButtonPressed = Player.GetButtonDown("Attack");
		ActionLeftButtonReleased = Player.GetButtonUp("Attack");
		
		ActionRightButton = Player.GetButton("Dash");
		ActionRightButtonPressed = Player.GetButtonDown("Dash");
		ActionRightButtonReleased = Player.GetButtonUp("Dash");
		
		DPadUp = Player.GetButton("Taunt1");
		DPadUpPressed = Player.GetButtonDown("Taunt1");
		DPadUpReleased = Player.GetButtonUp("Taunt1");

		AxisX = Player.GetAxisRaw("Horizontal");
		AxisY = Player.GetAxisRaw("Vertical");
	}	
}
