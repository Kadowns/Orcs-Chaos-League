using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntityState : EntityState {

	#region Fields
	
	public Transform RoofPoint;
	public Rigidbody JointRb;
	public float MoveSpeed = 500f;
	public GameObject CageDoor;
	public Animator SlotText;

	#endregion
	
	#region Properties
	
	public float DefaultRaised { get; set; }
	public float DefaultLowered{ get; set; }
	public SoundEffectPlayer Sfx{ get; set; }
	public PlayerController Controller { get; set; }
	public ShadowCaster ShadowCaster{ get; set; }
	public Renderer Cage{ get; set; }
	public Collider Col{ get; set; }
	public Rigidbody Rb{ get; set; }
	public Animator Animator{ get; set; }
	public GameObject Orc{ get; set; }
	public Vector3 DefaultPosition{ get; set; }
	public bool PlayerInside{ get; set; }
	public bool CanSpawn { get; set; }
	
	#endregion

	
}
