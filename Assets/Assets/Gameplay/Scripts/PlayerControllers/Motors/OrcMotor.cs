﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using OCL;
using UnityEngine;

[CreateAssetMenu]
public class OrcMotor : Motor {

	[SerializeField]
	private AudioClip m_burningSound;
	
	private FighterColors _colors;
	
	public override void Initialize(MovableEntity entity, InputController input) {
		var state = entity.State as OrcEntityState;
		
		state.ColliderRadiusY = entity.GetComponent<Collider>().bounds.size.y / 2;
		state.ColliderRadiusX = entity.GetComponent<Collider>().bounds.size.x / 2;

		state.Flash = entity.GetComponent<SpriteFlash>();
		state.Sfx = entity.GetComponent<SoundEffectPlayer>();
		state.Rb = entity.GetComponent<Rigidbody>();	
		state.Shadow = entity.GetComponent<ShadowCaster>();	
		
	}

	public override void Setup(MovableEntity entity, InputController input) {
		var state = entity.State as OrcEntityState;
		
		_colors = FighterColors.Instance;
		state.Rb.drag = state.AirDrag;
	}

	public override void Tick(MovableEntity entity, InputController input) {
		var state = entity.State as OrcEntityState;
		state.Grounded = Grounded(entity, state);

		var direction = input.Axis;
		
		if (Mathf.Abs(direction.x)  >= 0.03f|| Mathf.Abs(direction.y) >= 0.03f) {
			if (state.Grounded) {
				state.DistanceMoved += state.Rb.velocity.sqrMagnitude;
				if (state.DistanceMoved > 70f * 70f) {
					state.DistanceMoved = 0f;
					state.Sfx.PlaySFxByIndex(0, Random.Range(0.8f, 1.2f));
				}
			}
			Move(state, new Vector3(direction.x, 0, direction.y).normalized);		
		}
		if (input.ActionRightButtonPressed) {
			Dash(entity, state);
		}

		//Input de pulo----------------
		if (input.ActionDownButtonPressed) {
			Jump(state);
		} 
		//-----------------------------

		//Input de ataque--------------
		if (input.ActionLeftButtonPressed) {
			Attack(state);
		}
			
		if (input.ActionUpButtonPressed) {
			StartParry(entity, state);
		}
		else if (input.ActionUpButtonReleased) {
			StopParry(state);
		}
			
		//Input de Taunt
		if (input.DPadUpPressed) {
			Taunt(state);
		}

		

		//Temporizador do ataque (não permite que um unico ataque "ataque" varias vezes
		if (state.Attacking) {		
			if (state.SimpleAttack) {
				state.SimpleAttackTimer += Time.deltaTime;
				if (state.SimpleAttackTimer > state.AttackSpeed) {
					state.SimpleAttackTimer = 0f;

					state.ActualAttack.TrySFx(Random.Range(0.8f, 1.2f));
			
					if (state.ActualAttack.miniDash)
						state.Rb.AddForce(state.LastDir * 100, ForceMode.Impulse);
					
					ApplyDamage(entity, state);	

					state.TimeToCooldown = state.SimpleAttackCooldown;
					state.SimpleAttack = false;
					state.Attacking = false;	
				}
			}

			if (state.DropAttack) {		
				if (state.Grounded) {
					state.ActualAttack.TrySFx(Random.Range(0.8f, 1.2f));
					state.Sfx.PlaySFxByIndex(4, Random.Range(0.8f, 1.2f));
					ScreenEffects.Instance.ScreenShake(0.15f, 2.5f);
					ApplyDamage(entity, state);
					state.StompDrawback = true;
                    				
					state.TimeToCooldown = state.DropAttackCooldown;
					state.GravityScale = 1f;
					state.DropAttack = false;
					state.Attacking = false;
				}
			}		
		}
		
		
		//Temporizadores-------------
		//Cooldown
		if (state.Cooldown) {
			state.CooldownTimer += Time.deltaTime;
			if (state.CooldownTimer > state.TimeToCooldown) {
				state.CooldownTimer = 0;
				state.LastAttackId = 0;
				state.Cooldown = false;
				state.Attacking = false;
			}
		}
				
		if (state.Hurt) {
			state.HurtTimer += Time.deltaTime;			
			if (state.HurtTimer > state.HurtForSeconds) {
				state.HurtTimer = 0;
				state.Hurt = false;
			}
		}



		if (state.StompDrawback) {
			state.DrawbackTimer += Time.deltaTime;
			if (state.DrawbackForSeconds < state.DrawbackTimer) {
				state.DrawbackTimer = 0;
				state.StompDrawback = false;
			}
		}
		
		if (state.Dashed) {
			state.DashTimer += state.Grounded ? Time.deltaTime : Time.deltaTime * 0.25f;			
			if (state.DashTimer > state.DashCooldown) {
				state.DashTimer = 0;
				state.Dashed = false;
			}
		}

		if (state.Taunt) {
			state.TauntTimer += Time.deltaTime;
			if (state.TauntTimer > state.TauntForSeconds) {
				state.Taunt = false;
				state.TauntTimer = 0;
			}
		}

		if (state.Burning) {
			state.BurnTimer += Time.deltaTime;
			if (state.BurnTimer > state.TimeToBurn) {
				state.Burning = false;
				state.BurnTimer = 0;
			}
		}
	}

	public override void FixedTick(MovableEntity entity, InputController input) {
		var state = entity.State as OrcEntityState;
		

		if(state.Burning) {

			float speed = state.Grounded ? state.MoveSpeed * 1.5f : state.MoveSpeed * 0.5f;
			state.Rb.AddForce(state.LastDir * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);			
		}

		if (state.Grounded) {
			
			state.Shadow.SetLineEnabled(false);
			
			state.Rb.drag = state.GroundDrag;

		}
		else {
			
			state.Shadow.SetLineEnabled(true);
			
			state.Rb.drag = state.AirDrag;
			
			if (!state.Attacking) {
				if (state.Rb.velocity.y < -5.5f || (state.Rb.velocity.y > 0 && !input.ActionDownButton)) {
					state.GravityScale = state.FallMultiplier;
				}
				else {
					state.GravityScale = 1;
				}
			}		
		}
		ApplyGravity(entity, state);
	}

	public override void LateTick(MovableEntity entity, InputController input) {}

	private static void ApplyGravity(MovableEntity entity, OrcEntityState state) {
		Vector3 gravity = Physics.gravity * (state.GravityScale + 5);
		state.Rb.AddForce(gravity, ForceMode.Acceleration);
	}

	private void Kill(OrcEntityState state) {
		
		state.Rb.isKinematic = true;
		ResetToDefault(state);
		state.gameObject.SetActive(false);
		state.Controller.SubtractLife();
	}

	private bool Grounded(MovableEntity entity, OrcEntityState state) {
		RaycastHit hit;
		if (Physics.Raycast(entity.transform.position, Vector3.down, out hit, state.ColliderRadiusY + 0.05f, 1<<13)) {
			state.SequentialJumps = 0;
			entity.transform.SetParent(hit.transform.parent);
			if (state.DropAttack) {
				ScreenEffects.Instance.CreateStompParticles(state.transform.position);
				PlataformBehaviour plat = hit.collider.gameObject.GetComponent<PlataformBehaviour>();
				if (plat != null) {
					plat.Damage();
				}
			}

			return true;
		}

		entity.transform.SetParent(null);
		return false;
	}

	private void Attack(OrcEntityState state) {
		if (IsUnable(state))
			return;
		
		if (state.Grounded && state.LastAttackId < 3) {
			state.LastAttackId += 1;
			state.ActualAttack = state.Attacks[state.LastAttackId];
			state.SimpleAttack = true;
			state.Cooldown = false;	
			state.Attacking = true;
			
			
		}
		else if (!state.Cooldown && state.CanStomp) {
			state.LastAttackId = 0;
			state.ActualAttack = state.Attacks[state.LastAttackId];
			state.Sfx.PlaySFxByIndex(1, Random.Range(1f, 1.2f));
			state.DropAttack = true;
			state.DropAttackForce = -DistanceToGround(state.transform.position) * state.DropAttackForceMultiplier;
			
			state.GravityScale = state.DropAttackGravity;
			state.Attacking = true;	
		}
	}
	
	private void ApplyDamage(MovableEntity entity, OrcEntityState state) {
	
		Collider[] orcsCols = Physics.OverlapSphere(entity.transform.position, state.ActualAttack.range, 1<<LayerMask.NameToLayer("Players"));
		Collider[] rockCols = Physics.OverlapSphere(entity.transform.position, state.ActualAttack.range * 1.5f, 1<<LayerMask.NameToLayer("Rocks"));
		
		if (orcsCols.Length > 0) {
			for (int i = 0; i < orcsCols.Length; i++) {
				//se o maluco estiver dentro do circulo e for um player, ele é arremessado
				if (orcsCols[i].gameObject != entity.gameObject) {

					var otherEntity = orcsCols[i].gameObject.GetComponent<MovableEntity>();
					var otherMotor = otherEntity.Motor as OrcMotor;
					var otherState = otherEntity.State as OrcEntityState;
         
					if (otherState.Grounded || state.SimpleAttack) {
						Vector3 dir = (otherEntity.transform.position - entity.transform.position).normalized;

						if (!otherState.CanCounter) {
                            state.Controller.Hit(otherState.Controller);
							otherMotor.Damage(otherState, dir, state.ActualAttack.force,
								state.ActualAttack.hurtForSeconds * (!otherState.Grounded && state.SimpleAttack ? 5 : 1),
								state.ActualAttack.knockBack,
								state.ActualAttack.knockUp,
								state.Controller.PlayerNumber);
							if (state.ActualAttack.screenShake)
								ScreenEffects.Instance.ScreenShake(0.1f, 1f);
							
							state.ActualAttack.HitSFx(Random.Range(0.8f, 1.2f));
						}
						else {
							otherMotor.DoCounter(otherState);
							Damage(state, -dir, state.ActualAttack.force, state.TimeToStun, true, true, otherState.Controller.PlayerNumber);
						} 			
					}						
				}
			}
		}

		if (rockCols.Length > 0) {
			foreach (var rock in rockCols) {
				state.ActualAttack.HitSFx(Random.Range(0.8f, 1.2f));
				var throwable = rock.GetComponent<IThrowable>();
				if (throwable != null) {

					var closest = ClosestOrc(entity);

					Vector3 dir = ((closest != Vector3.zero ? closest : rock.transform.position) - entity.transform.position)
						.normalized;
					rock.transform.position = entity.transform.position + dir * 5 + Vector3.up * 2;

					throwable.Throw(dir, state.Controller.PlayerNumber);
				}
			}
		}
		state.Cooldown = true;
	}

	public void CollectItem(OrcEntityState state, CollectableItem.ItemType type) {
		switch (type) {
			case CollectableItem.ItemType.OrcHead:
				state.Controller.GotKill();
				break;
		}
	}

	public void DoCounter(OrcEntityState state) {
		ScreenEffects.Instance.FreezeFrame(0.08f);
		ScreenEffects.Instance.ScreenShake(0.1f, 0.5f);
		state.Sfx.PlaySFxByIndex(5, Random.Range(0.8f, 1.2f));
		state.Countered = true;
		StopParry(state);
	}
	
	private Vector3 ClosestOrc(MovableEntity entity) {
		Collider[] cols = Physics.OverlapSphere(entity.transform.position, 90, (1<<11 | 1<<10));
		Vector3 closest = Vector3.zero;
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			if (col.gameObject == entity.gameObject)
				continue;
			
			float dist = (col.transform.position - entity.transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				closest = col.transform.position;
			}
		}
		return closest;
	}
	
	public void Damage(OrcEntityState state, Vector3 dir, float force, float hurtTime, bool knockBack, bool knockUp, int attackerNumber) {
		if (!state.gameObject.activeInHierarchy)
			return;
		
		if (!state.Parrying) {
			state.Sfx.PlaySFxByIndex(2, Random.Range(0.9f, 1.1f));
			WasHit(state, hurtTime);
			AddDamage(state, (int) ((state.Damage + force) * 0.07f), attackerNumber);
			ScreenEffects.Instance.CreateHitParticles(state.transform, dir);
			
			FlashColor(state, _colors.HurtColor, 0.2f, 1f);
			
			if (knockUp)
				dir = (dir + (Vector3.up * 0.5f)).normalized;

			if (knockBack)
				state.Rb.AddForce(dir * (force + state.Damage), ForceMode.Impulse);
				
		} else {
			
			state.Sfx.PlaySFxByIndex(3, Random.Range(0.8f, 1.2f));
			FlashColor(state, _colors.HurtColor, 0.2f, 0.6f);
		}		    
		
		Vibrate(state, 0, (force + state.Damage) * 0.1f , 0.1f);		
	   
	}
	
	private void Taunt(OrcEntityState state) {
		if (IsUnable(state))
			return;
		state.Taunt = true;
	}
	
	private void Vibrate(OrcEntityState state, int motorIndex, float motorLevel, float duration) {
		state.Controller.Vibrate(motorIndex, motorLevel, duration);
	}

	public void FellOnLava(OrcEntityState state) {
		if (state.HasShield && !state.ShieldConsumed) {
			state.ShieldConsumed = true;
			state.Rb.AddForce(Vector3.up * 300, ForceMode.Impulse);
			ScreenEffects.Instance.CreateShield(state.transform);
			return;
		}
		
		if (state.Damage < 100) {
			Burn(state, 50, 2.5f, Vector3.up, 300, state.Controller.LastAttackerNumber);
		}
		else {
			Kill(state);
			AudioController.Instance.Play(m_burningSound, AudioController.SoundType.GlobalSoundEffect, false, 1, 0);
			ScreenEffects.Instance.CreateDeadOrc(state.transform.position);
		}
	}
	
	public void Burn(OrcEntityState state, int damage, float timeToBurn, Vector3 dir, float knockBackForce, int attackerID) {
		
		Vibrate(state, 1, damage / 50f, timeToBurn);
		state.Rb.AddForce(dir * knockBackForce, ForceMode.Impulse);
		state.TimeToBurn = timeToBurn;
		state.Hurt = false;
		state.Burning = true;
		AddDamage(state, damage, attackerID);
		WasHit(state, 0);
		
		AudioController.Instance.Play(m_burningSound, AudioController.SoundType.GlobalSoundEffect, false, 1, 0);
		ScreenEffects.Instance.CreateBurningParticles(state.transform, timeToBurn);	
	}
	
	private void AddDamage(OrcEntityState state, int amount, int attackerID) {
		state.Damage = state.Damage + amount > 999 ? 999 : state.Damage + amount;
		state.Controller.WasHit(attackerID);
		if (state.Damage > 250) {
			Kill(state);
			ScreenEffects.Instance.CreateBloodSplashParticles(state.transform.position);
			ScreenEffects.Instance.FreezeFrame(0.1f);
		}	else if (state.Damage > 100) {
			state.Flash.SetRedAmount(state.Damage);
		}
	}

	private void WasHit(OrcEntityState state, float hurtTime) {
		state.HurtForSeconds = hurtTime;	
		state.Hurt = true;
		state.Taunt = false;
		state.CanStomp = false;
		state.TauntTimer = 0;
		state.Rb.isKinematic = false;

        state.AttackSpeed = Mathf.Lerp(state.InitialAttackSpeed, 0.05f, (float)state.Damage / 100);
		state.Attacking = false;
		state.LastAttackId = 0;
		state.SimpleAttack = false;
		state.DropAttack = false;
		state.SimpleAttackTimer = 0;
	}
	
	private static float DistanceToGround(Vector3 from) {
		float dist = 0;
		RaycastHit hit;
		if (Physics.Raycast(from, Vector3.down, out hit, float.PositiveInfinity)) {
			dist = hit.point.y - from.y;
		}
		return dist + 0.1f;
	}
	
	private void Jump(OrcEntityState state) {
		
		if ((!state.Grounded && state.SequentialJumps >= state.MaxAirJumps)|| IsUnable(state)) 
			return;

		var jumpForce = state.JumpForce;

		if (!state.Grounded) {			
			jumpForce *= 0.6f;
			ScreenEffects.Instance.CreateDashParticles(state.transform.position + Vector3.down, state.transform, Vector3.down);
		}
		else {
			state.Sfx.PlaySFxByIndex(1, Random.Range(0.8f, 1.2f));
		}

		state.SequentialJumps++;
		
		
		Vector3 momentum = state.Dashed ? Vector3.ClampMagnitude(state.Rb.velocity * 4, 100) : state.Rb.velocity * 4;
		
		state.Rb.AddForce(momentum.x, jumpForce, momentum.z, ForceMode.Impulse);
		
		state.CanStomp = true;
	}
	
	private void Move(OrcEntityState state, Vector3 dir) {
		state.LastDir = dir;
		if (IsUnable(state))
			return;
	
		float speed = state.Grounded ? state.MoveSpeed : state.MoveSpeed * 0.35f;
		
		
		state.Rb.AddForce(dir * speed * Time.deltaTime, ForceMode.VelocityChange);	
	}
	
	public bool IsUnable(OrcEntityState state) {
		return
			state.Hurt || state.Burning || state.Parrying || state.Attacking || state.StompDrawback || state.Taunt || state.Dashing;
	}	
	
	private void Dash(MovableEntity entity, OrcEntityState state) {
		if (IsUnable(state) || state.Dashed)
			return;
		
		if (state.DashRoutine != null) {
			state.StopCoroutine(state.DashRoutine);
		}

		state.DashRoutine = state.StartCoroutine(DoDash(entity, state, state.LastDir, 0.1f));
		ScreenEffects.Instance.CreateDashParticles(state.transform, -state.LastDir);
	}	
		
	private void StartParry(MovableEntity entity, OrcEntityState state) {
		if (IsUnable(state) || !state.Grounded)
			return;

		state.ParryRoutine = state.StartCoroutine(DoParry(state));
	}

	private void StopParry(OrcEntityState state) {
		state.StopParry = true;
	}

	private void BreakParry(OrcEntityState state,int attackerNumber,  Vector3 dir) {
		if (state.ParryRoutine != null) {
			state.StopCoroutine(state.ParryRoutine);
		}
		state.StopParry = false;
		state.Parrying = false;
		state.Countered = false;
		state.CanCounter = false;
		Damage(state, dir, 150, 1, true, true, attackerNumber);
	}

	
	private IEnumerator DoParry(OrcEntityState state) {
		state.Rb.velocity *= 0.5f;
		FlashColor(state, _colors.AttackPreparationColor, state.TimeToCounter, 0.5f);
		state.CanCounter = true;
		state.Parrying = true;
		state.StopParry = false;
		float timer = 0;
		while (timer < state.MinimumParryTime || !state.StopParry) {
			timer += Time.deltaTime;
			if (state.CanCounter) {
				if (timer > state.TimeToCounter) {
					SetTintColor(state, _colors.ParryColor);
					state.CanCounter = false;
				}
			}
			yield return null;
		}
		
		SetTintColor(state, Color.white);
		state.StopParry = false;
		state.Parrying = false;
		state.CanCounter = false;
	}
	
	private IEnumerator DoDash(MovableEntity entity, OrcEntityState state, Vector3 dir, float timeToDash) {
		state.Dashing = true;
		state.Rb.isKinematic = true;
		float timer = 0;
		while (timer < timeToDash) {
			
			RaycastHit hit;
			if (Physics.SphereCast(state.transform.position, 1, dir, out hit, state.DashSpeed * Time.deltaTime)){
				
				state.transform.position += dir * (hit.distance - state.ColliderRadiusX * 2) * Time.deltaTime;
				if (hit.collider.CompareTag("Player") && hit.collider.gameObject != state.gameObject) {
					
					var otherEntity = hit.collider.gameObject.GetComponent<MovableEntity>();
					var otherMotor = otherEntity.Motor as OrcMotor;
					var otherState = otherEntity.State as OrcEntityState;
					if (otherMotor != null) {
						var d = (otherEntity.transform.position - state.transform.position).normalized;
						state.Sfx.PlaySFxByIndex(3, Random.Range(0.8f, 1.2f));				
						if (otherState.Parrying) {
							otherMotor.BreakParry(otherState,state.Controller.PlayerNumber, (otherState.transform.position - state.transform.position).normalized);
							state.Sfx.PlaySFxByIndex(2, Random.Range(0.8f, 1.2f));				
							ScreenEffects.Instance.FreezeFrame(0.15f);
						} else {
							otherMotor.Damage(otherState, d, 100, 1f, true, true, state.Controller.PlayerNumber);
							ScreenEffects.Instance.FreezeFrame(0.08f);
						}
					}
				}
				break;
			}
			state.transform.position += dir * state.DashSpeed * Time.deltaTime;		
			timer += Time.deltaTime;
			yield return null;
		}
		state.Rb.isKinematic = false;
		state.Dashing = false;
		state.Dashed = true;
	}
	
	public void FlashColorAfterSeconds(OrcEntityState state, Color c, float wait, float timeToFlash, float intensity) {
		state.Flash.ImpactFlashAfterSeconds(c, wait, timeToFlash, intensity);
	}
	
	public void FlashColor(OrcEntityState state, Color c1, float timeToFlash, float intensity) {
		state.Flash.ImpactFlash(c1, timeToFlash, intensity);
	}

	public void SetTintColor(OrcEntityState state, Color c) {
		state.Flash.SetTintColor(c);
	}
	
	public void ExitBox(OrcEntityState state, Vector3 boxPosition) {	
		state.gameObject.SetActive(true);
		state.transform.position = boxPosition + Vector3.up * 10;
		ResetToDefault(state);
		state.Rb.AddForce(0, state.JumpForce * 2, 0, ForceMode.Impulse);
		state.Sfx.PlaySFxByIndex(1, Random.Range(0.8f, 1.2f));
		
		ScreenEffects.Instance.ScreenShake(0.1f, 1f);
	}

	public void ResetToDefault(OrcEntityState state) {
		state.Damage = 0;
		state.Hurt = false;	
		state.Burning = false;
		state.StompDrawback = false;
		state.Taunt = false;
		state.Dashing = false;
		state.CanStomp = false;
		state.Rb.isKinematic = false;
		state.Flash.ResetToDefault();
		
		var particles = state.GetComponentsInChildren<ParticleSystem>();
		foreach (var particle in particles) {
			particle.gameObject.SetActive(false);
		}
		
		state.StopAllCoroutines();
		state.LastAttackId = 0;
        state.AttackSpeed = state.InitialAttackSpeed;
        state.StopParry = false;
	    state.Attacking = false;
		state.Parrying = false;
		state.CanCounter = false;
		state.Countered = false;
	}
}
