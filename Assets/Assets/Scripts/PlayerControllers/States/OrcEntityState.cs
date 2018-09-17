using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrcEntityState : EntityState {
	
	#region Fields

	#region MainFields
	
	public float AirDrag = 7f;
	public float GroundDrag = 14f;
	public float FallMultiplier = 10f;
	public float MoveSpeed = 20f;
	public float DashSpeed = 150f;
	public float JumpForce = 30f;
	public float DrawbackForSeconds = 0.5f;
	public float DashCooldown = 2f;
	public float TauntForSeconds = 1f;
	
	#endregion

	#region AttackFields
	//Ataque	
	public float SimpleAttackCooldown = 1f;
	public float DropAttackCooldown = 1f;
	public float InitialAttackSpeed = 0.2f;
	
	public float DropAttackForceMultiplier = 2;
	public float DropAttackGravity = 300f;
	public float MinimumParryTime = 0.5f;
	public float TimeToCounter = 1f;
	public float TimeToStun = 1f;
	
	#endregion
	
	#endregion
	
	#region Properties
	
	#region MainProperties
	
	public int Damage { get; set; }

	public Rigidbody Rb { get; set; }

	public SpriteFlash Flash { get; set; }

	public PlayerController Controller { get; set; }

	public SoundEffectPlayer Sfx { get; set; }

	public ShadowCaster Shadow { get; set; }

	public Vector3 Velocity {
		get {
			return new Vector3(Rb.velocity.x, 0, Rb.velocity.z);
		}
	}
	
	public Color PlayerColor { get; set; }

	public Vector3 LastPos { get; set; }

	public Vector3 LastDir { get; set; }

	public Coroutine DashRoutine { get; set; }
	
	public Coroutine ParryRoutine { get; set; }

	public float GravityScale { get; set; }

	public float ColliderRadiusY { get; set; }

	public float DistanceMoved { get; set; }

	public float ColliderRadiusX { get; set; }

	public bool Grounded { get; set; }
	
	public bool DoubleJump { get; set; }

	public bool Dashed { get; set; }

	public bool Attacking { get; set; }

	public bool Parrying { get; set; }

	public bool Taunt { get; set; }

	public bool CanStomp { get; set; }

	public bool Dashing { get; set; }

	public bool Burning { get; set; }

	public bool StompDrawback { get; set; }

	public float TimeToBurn { get; set; }

	public float BurnTimer { get; set; }

	public float DashTimer { get; set; }

	public float DrawbackTimer { get; set; }

	public float TauntTimer { get; set; }

	public bool Hurt { get; set; }

	public float HurtTimer { get; set; }

	#endregion
	
	#region AttackProperties

	public Attack ActualAttack { get; set; }
	
	public float AttackSpeed { get; set; }

	public int LastAttackId { get; set; }

	public bool DropAttack { get; set; }

	public bool SimpleAttack { get; set; }

	public bool Cooldown { get; set; }

	public bool CanCounter { get; set; }

	public bool Countered { get; set; }

	public bool StopParry { get; set; }

	public float TimeToCooldown { get; set; }

	public float CooldownTimer { get; set; }

	public float SimpleAttackTimer { get; set; }

	public float DropAttackForce { get; set; }

	public float HurtForSeconds { get; set; }
	
	#endregion
	
	#endregion
	
}
