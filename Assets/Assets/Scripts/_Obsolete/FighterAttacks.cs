using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(FighterBehaviour))]
public class FighterAttacks : MonoBehaviour {

	
	


	public List<Attack> _attacks;

	private Attack _actualAttack;
	
	private Rigidbody _rb;
	private FighterBehaviour _fighter;
	private CustomGravity _gravity;
	private SoundEffectPlayer _sfx;
	private FighterColors _colors;

	private int _lastAttackId;

	private bool _dropAttack, _simpleAttack, _coolDown;
	
	//Temporizadores
	private bool _attacking, _parrying, _canCounter, _countered, _stopParry;
	
	//Ataque	
	[SerializeField] private float _simpleAttackCooldown = 1f;
	[SerializeField] private float _dropAttackCooldown = 1f;
	private float _timeToCooldown = 1f;
	private float _cooldownTimer = 0f;
	
	//Ataque simples	
	[SerializeField] private float _attackSpeed = 0.25f;
	private float _simpleAttackTimer = 0f;
	
	//Drop Attack
	[SerializeField] private float _dropAttackForceMultiplier = 2;
	[SerializeField] private float _dropAttackGravity = 300f;
	private float _dropAttackForce;
	
	
	//Parry
	[SerializeField] private float _minimumParryTime = 0.5f;
	[SerializeField] private float _timeToCounter = 1f;

    [SerializeField] private float _timeToStun = 1f;
	private float _hurtForSeconds = 0;
    private float _hurtTimer = 0f;

	private void Start() {
		_fighter = GetComponent<FighterBehaviour>();
		_rb = GetComponent<Rigidbody>();
		_sfx = GetComponent<SoundEffectPlayer>();
		_colors = FighterColors.Instance;
		foreach (var atk in _attacks) {
			atk.Start(gameObject);
		}
	
		//--------------------------------------------------------
		_gravity = GetComponent<CustomGravity>();	
	}

	public void ResetToDefault() {
		StopAllCoroutines();
		_lastAttackId = 0;
		_stopParry = false;
		_attacking = false;
		_parrying = false;
		_canCounter = false;
		_countered = false;
	}
	
	private void Update() {
		//Temporizador do ataque (não permite que um unico ataque "ataque" varias vezes
		if (_attacking) {		
			if (_simpleAttack) {
				_simpleAttackTimer += Time.deltaTime;
				if (_simpleAttackTimer > _attackSpeed) {
					_simpleAttackTimer = 0f;

					_actualAttack.TrySFx(Random.Range(0.8f, 1.2f));
			
					if (_actualAttack.miniDash)
						_rb.AddForce(_fighter.LastDir() * 100, ForceMode.Impulse);
					
                    ApplyDamage();	

					_timeToCooldown = _simpleAttackCooldown;
					_simpleAttack = false;
					_attacking = false;	
				}
			}

			if (_dropAttack) {		
				if (_fighter.IsGrounded()) {
					_actualAttack.TrySFx(Random.Range(0.8f, 1.2f));
					ScreenEffects.Instance.ScreenShake(0.15f, 2.5f);
                    ApplyDamage();
					_fighter.StompDrawback();
					DamagePlataform();
                    				
					_timeToCooldown = _dropAttackCooldown;
					_gravity.GravityScale = 1f;
					_dropAttack = false;
					_attacking = false;
				}
			}		
		}
		
		//Cooldown
		if (_coolDown) {
			_cooldownTimer += Time.deltaTime;
			if (_cooldownTimer > _timeToCooldown) {
				_cooldownTimer = 0;
				_lastAttackId = 0;
				_coolDown = false;
				_attacking = false;
			}
		}
	}

	private void DamagePlataform() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, _fighter.ColliderDistanceToGround())) {
			if(hit.collider.CompareTag("Ground")) {
				ScreenEffects.Instance.CreateStompParticles(transform.position);
				PlataformBehaviour plat = hit.collider.gameObject.GetComponent<PlataformBehaviour>();
				if (plat != null) {
					plat.Damage();
				}			
			}
		}
	}

	public void DoAttack() {
		
		if (_fighter.IsGrounded() && _lastAttackId < 3) {
			_lastAttackId += 1;
			_actualAttack = _attacks[_lastAttackId];
			_simpleAttack = true;
			_coolDown = false;	
			_attacking = true;
			
			
		}
		else if (!_coolDown && _fighter.CanStomp()) {
			_lastAttackId = 0;
			_actualAttack = _attacks[_lastAttackId];
			_sfx.PlaySFxByIndex(1, Random.Range(1f, 1.2f));
			_dropAttack = true;
			_dropAttackForce = -DistanceToGround() * _dropAttackForceMultiplier;
			
			_gravity.GravityScale = _dropAttackGravity;
			_attacking = true;	
		}
	}

	public bool CanCounter() {
		return _canCounter;
	}

	public void StartParry() {	
		StartCoroutine(DoParry());
	}

	public void StopParry() {
		_stopParry = true;
	}

	private IEnumerator DoParry() {
		_rb.velocity = new Vector3(_rb.velocity.x * 0.5f, _rb.velocity.y, _rb.velocity.z * 0.5f);
		_fighter.FlashColor(_colors.AttackPreparationColor, _timeToCounter, 0.5f);
		_canCounter = true;
		_parrying = true;
		float timer = 0;
		while (timer < _minimumParryTime || !_stopParry) {
			timer += Time.deltaTime;
			if (_canCounter) {
				if (timer > _timeToCounter) {
					_fighter.SetTintColor(_colors.ParryColor);
					_canCounter = false;
				}
			}
			yield return null;
		}
		_fighter.SetTintColor(Color.white);
		_stopParry = false;
		_parrying = false;
		_countered = false;
		_canCounter = false;
	}

	private void ApplyDamage() {

		
		Collider[] cols = Physics.OverlapSphere(transform.position, _actualAttack.range);
		
		if (cols.Length > 0) {
			for (int i = 0; i < cols.Length; i++) {
				//se o maluco estiver dentro do circulo e for um player, ele é arremessado
				if (cols[i].CompareTag("Player") && cols[i].gameObject != gameObject) {
					FighterBehaviour orc = cols[i].gameObject.GetComponent<FighterBehaviour>();
         
					if (orc.IsGrounded()) {
						Vector3 dir = (orc.transform.position - transform.position).normalized;

						if (!orc.CanCounter()) {
							
							orc.Damage(dir, _actualAttack.force, _actualAttack.hurtForSeconds, _actualAttack.knockBack, _actualAttack.knockUp);
							if (_actualAttack.screenShake)
								ScreenEffects.Instance.ScreenShake(0.1f, 1f);
							
							_actualAttack.HitSFx(Random.Range(0.8f, 1.2f));
						}
						else {
							ScreenEffects.Instance.FreezeFrame(0.08f);
							ScreenEffects.Instance.ScreenShake(0.1f, 0.5f);
							GlobalAudio.Instance.PlayByIndex(4);
							_fighter.Damage(-dir, _actualAttack.force, _timeToStun, true, true);
							orc.Counter();
						} 			
					}						
				}
				else if (cols[i].CompareTag("LavaRock")) {

					Vector3 closest = ClosestOrc();
					Vector3 dir = ((closest != Vector3.zero ? closest : cols[i].transform.position) - transform.position).normalized;

					cols[i].transform.position = transform.position + dir * 5;
					cols[i].GetComponent<LavaRockBehaviour>().Throw(dir);
				}	
			}
		}
		_coolDown = true;
	}

	public Vector3 ClosestOrc() {
		Collider[] cols = Physics.OverlapSphere(transform.position, 90, 1<<11);
		Vector3 closest = Vector3.zero;
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			if (col.gameObject == gameObject)
				continue;
			
			float dist = (col.transform.position - transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				closest = col.transform.position;
			}
		}
		return closest;
	}

	public void WasHit(float damage) {
		_attackSpeed = 1f / (4 + damage / 25);
		_attacking = false;
		_lastAttackId = 0;
		_simpleAttack = false;
		_dropAttack = false;
		_simpleAttackTimer = 0;
	}

	public void Counter() {
		_countered = true;
	}

	
	public int GetAttackId() {
		return _lastAttackId;
	}
	
	private float DistanceToGround() {
		return _fighter.DistanceToGround();
	}
	
	public bool IsAttacking() {
		return _attacking;
	}

	public bool IsParrying() {
		return _parrying;
	}

	public bool IsCountering() {
		return _countered;
	}

	public bool Cooldown() {
		return _coolDown;
	}		
}
