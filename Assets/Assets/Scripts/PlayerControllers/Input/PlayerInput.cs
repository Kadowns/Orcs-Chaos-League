using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerControllers.Input;
using Rewired;
using UnityEngine;

[CreateAssetMenu(menuName = "InputSource/PlayerInput")]
public class PlayerInput : InputSource {
	
	[HideInInspector] public Player Player;

	public override void Tick(InputController input) {

		if (Player == null)
			return;
		
		input.CenterButtonPresssed = Player.GetButtonDown("Pause");
		
		input.ActionDownButton = Player.GetButton("Jump");
		input.ActionDownButtonPressed = Player.GetButtonDown("Jump");
		input.ActionDownButtonReleased = Player.GetButtonUp("Jump");
			
		input.ActionUpButton = Player.GetButton("Parry");
		input.ActionUpButtonPressed = Player.GetButtonDown("Parry");
		input.ActionUpButtonReleased = Player.GetButtonUp("Parry");
		
		input.ActionLeftButton = Player.GetButton("Attack");
		input.ActionLeftButtonPressed = Player.GetButtonDown("Attack");
		input.ActionLeftButtonReleased = Player.GetButtonUp("Attack");
		
		input.ActionRightButton = Player.GetButton("Dash");
		input.ActionRightButtonPressed = Player.GetButtonDown("Dash");
		input.ActionRightButtonReleased = Player.GetButtonUp("Dash");
		
		input.DPadUp = Player.GetButton("Taunt1");
		input.DPadUpPressed = Player.GetButtonDown("Taunt1");
		input.DPadUpReleased = Player.GetButtonUp("Taunt1");

		input.Axis = new Vector2(Player.GetAxisRaw("Horizontal"), Player.GetAxisRaw("Vertical"));
	}	
}
