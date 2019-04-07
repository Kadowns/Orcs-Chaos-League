using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class CursorMovement : MonoBehaviour
{
	public GameObject[] coOBJ = new GameObject[8];
	
	private Player PiM;
	
	public float speed = 10f;

	public int PlayerID = 0;
	
	public void Start ()
	{
		PiM = ReInput.players.GetPlayer(PlayerID);
	}
	
	public void Update () {	
		PiM = ReInput.players.GetPlayer(PlayerID);
		
		float UpDown = PiM.GetAxis("UIVertical") * speed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z);
		
		float LeftRight = PiM.GetAxis("UIHorizontal") * speed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x + LeftRight, transform.position.y, transform.position.z);
	}
}