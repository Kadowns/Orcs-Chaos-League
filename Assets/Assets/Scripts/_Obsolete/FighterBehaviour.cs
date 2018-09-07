using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class FighterBehaviour : MonoBehaviour {

	
	//SerializeField permite que a variavel seja 
	//privada mas ainda assim apareça no editor da unity
	[SerializeField] private float _moveSpeed = 20f;
	[SerializeField] private float _dashSpeed = 150f;
	[SerializeField] private float _jumpForce = 30f;
	[SerializeField] private float _hurtForSeconds;
	[SerializeField] private float _drawbackForSeconds = 0.5f;
	[SerializeField] private float _dashCooldown = 2f;
	[SerializeField] private float _tauntForSeconds = 1f;
	

	private int _damage;
	
	private Rigidbody _rb;
	private ObsoletePlayerInput _input;

	private FighterColors _colors;
	private SpriteFlash _flash;
	private FighterAttacks _atks;
	private SoundEffectPlayer _sfx;
	private ShadowCaster _shadowCaster;
	private Vector3 _lastPos;
	private Vector3 _lastDir;

	private Coroutine _dashRoutine;
	
	private float _colliderRadiusY, _distanceMoved, _colliderRadiusX;

	private bool _grounded, _dashed, _taunt, _canStomp, _dashing, _doubleJump;
	
	//Temporizadores
	private bool _burning, _stompDrawback;
	private float _timeToBurn = 2.5f, _burnTimer = 0, _dashTimer = 0, _drawbackTimer = 0, _tauntTimer = 0;
	private bool _hurt;
	private float _hurtTimer = 0;


	private void Awake() {

		//Variavel pra pegar a distancia do centro do player até a base dele
		//(é usada na hora de detectar se o player ta no chão ou não
		_colliderRadiusY = GetComponent<Collider>().bounds.size.y / 2;
		_colliderRadiusX = GetComponent<Collider>().bounds.size.x / 2;

		_flash = GetComponent<SpriteFlash>();
		_sfx = GetComponent<SoundEffectPlayer>();
		_rb = GetComponent<Rigidbody>();
		_atks = GetComponent<FighterAttacks>();	
		_shadowCaster = GetComponent<ShadowCaster>();	
		
	}
	
	private void Start() {
		_colors = FighterColors.Instance;
		_input = GetComponentInParent<ObsoletePlayerInput>();
		_input.UpdateDamage(_damage);
		_rb.drag = 7f;
	}

	private void Update() {

		_grounded = Grounded();
		//Efeito sonoro de passos
		if (_input.IsMoving() && _grounded) {
			_distanceMoved += _rb.velocity.sqrMagnitude;
			if (_distanceMoved > 70f * 70f) {
				_distanceMoved = 0f;
				_sfx.PlaySFxByIndex(0, Random.Range(0.8f, 1.2f));
			}
		}

		//Temporizador do dano
		if (_hurt) {
			_hurtTimer += Time.deltaTime;			
			if (_hurtTimer > _hurtForSeconds) {
				_hurtTimer = 0;
				_hurt = false;
			}
		}

		if (_stompDrawback) {
			_drawbackTimer += Time.deltaTime;
			if (_drawbackForSeconds < _drawbackTimer) {
				_drawbackTimer = 0;
				_stompDrawback = false;
			}
		}
		
		if (_dashed) {
			_dashTimer += _grounded ? Time.deltaTime * 4 : Time.deltaTime;			
			if (_dashTimer > _dashCooldown) {
				_dashTimer = 0;
				_dashed = false;
			}
		}

		if (_taunt) {
			_tauntTimer += Time.deltaTime;
			if (_tauntTimer > _tauntForSeconds) {
				_taunt = false;
				_tauntTimer = 0;
			}
		}
	}

	private void FixedUpdate() {
		if(_burning) {
			_burnTimer += Time.deltaTime;
			
			float speed = _grounded ? _moveSpeed * 1.5f: _moveSpeed * 0.5f;

			_rb.AddForce(_lastDir * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
			if (_burnTimer > _timeToBurn) {
				_burning = false;
				_burnTimer = 0;
			}
		}

		if (_grounded) {
			
			_shadowCaster.SetLineEnabled(false);
			
			_rb.drag = 14;

		}
		else if (!_grounded) {
			
			_shadowCaster.SetLineEnabled(true);
			
			_rb.drag = 7f;
			
		}
	}

	public void ExitBox() {
		ResetToDefault();
		gameObject.SetActive(true);
		ScreenEffects.Instance.ScreenShake(0.1f, 1f);
		_rb.AddForce(0, _jumpForce * 2, 0, ForceMode.Impulse);
		_sfx.PlaySFxByIndex(1, Random.Range(0.8f, 1.2f));
	}


	public void ResetToDefault() {
		_damage = 0;
		_hurt = false;	
		_burning = false;
		_atks.ResetToDefault();
		_stompDrawback = false;
		_taunt = false;
		_dashing = false;
		_canStomp = false;
		_flash.ResetToDefault();
		
		var particles = GetComponentsInChildren<ParticleSystem>();
		foreach (var particle in particles) {
			particle.gameObject.SetActive(false);
		}
	}

	//Morre se encostar na lava ):
	private void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag("Lava")) {		
			if (_damage < 100) {
				Burn(50, 2.5f, Vector3.up, 300);
			}
			else {
				GlobalAudio.Instance.PlayByIndex(0);
				ScreenEffects.Instance.CreateDeadOrc(transform.position);
				gameObject.SetActive(false);
				_input.SubtractLife();
			}		
		}
	}

	public void Burn(int damage, float timeToBurn, Vector3 dir, float knockBackForce) {
		
		Vibrate(0, damage / 50f, timeToBurn);
		_rb.AddForce(dir * knockBackForce, ForceMode.Impulse);
		_timeToBurn = timeToBurn;
		_hurt = false;
		_burning = true;
		AddDamage(damage);
		_atks.WasHit(_damage);
		
		GlobalAudio.Instance.PlayByIndex(0);
		ScreenEffects.Instance.CreateBurningParticles(transform, timeToBurn);	
	}

	public bool IsDrawbacked() {
		return _stompDrawback;
	}

	public void StompDrawback() {
		_stompDrawback = true;
		_canStomp = false;
	}

	public void Taunt() {
		if (IsUnable())
			return;
		_taunt = true;
	}

	public bool IsTaunting() {
		return _taunt;
	}

	public void Counter() {
		_atks.Counter();
	}

	public bool IsCountering() {
		return _atks.IsCountering();
	}
	
	private void AddDamage(int amount) {
		_damage = _damage + amount > 999 ? 999 : _damage + amount;
		_flash.SetRedAmount(_damage);
		_input.UpdateDamage(_damage);
	}

	public int GetDamage() {
		return _damage;
	}

	//Verifica se tem alguma coisa diretamente embaixo do player
	private bool Grounded() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, _colliderRadiusY + 0.05f)) {
			_doubleJump = false;
			if (hit.collider.CompareTag("Lava"))
				return false;

			if (hit.collider.CompareTag("Ground")) {
				transform.SetParent(hit.transform);	
			}				
			
			return true;
		}
		transform.SetParent(null);	
		return false;
	}

	public Vector3 Velocity {
		get { return _rb.velocity; }
	}

	public float ColliderDistanceToGround() {
		return _colliderRadiusY;
	}
	
	public bool IsGrounded() {
		return _grounded;
	}

	public GameObject GetParent() {
		return _input.gameObject;
	}
	
	public float DistanceToGround() {
		float dist = 0;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity)) {
			dist = hit.point.y - transform.position.y;
		}
		return dist + 0.1f;
	}

	//Movimento
	public void Move(Vector3 dir) {
		_lastDir = dir;
		if (IsUnable())
            return;
	
		float speed = _grounded ? _moveSpeed : _moveSpeed * 0.35f;
		
		
		_rb.AddForce(dir * speed * Time.deltaTime, ForceMode.VelocityChange);	
	}

	//Pulo (só vai pular caso tenha alguma coisa diretamente embaixo do player)
	public void Jump() {
		if ((!IsGrounded() && _doubleJump)|| IsUnable()) 
            return;


		if (!IsGrounded()) {
			_doubleJump = true;
			ScreenEffects.Instance.CreateDashParticles(transform.position + Vector3.down, transform, Vector3.down);
		}
		else {
			_sfx.PlaySFxByIndex(1, Random.Range(0.8f, 1.2f));
		}
			
		
		Vector3 momentum = _dashed ? Vector3.ClampMagnitude(_rb.velocity * 4, 100) : _rb.velocity * 4;
		
		_rb.AddForce(momentum.x, _jumpForce, momentum.z, ForceMode.Impulse);
		
		_canStomp = true;
	}

	public bool IsUnable() {
		return _hurt || _burning || _atks.IsParrying() || _atks.IsAttacking() || _stompDrawback || _taunt || _dashing;
	}	
	
	public bool IsAttacking() {
		return _atks.IsAttacking();
	}

	public bool IsHurt() {
		return _hurt;
	}

	public int GetPlayerNumber() {
		return _input.GetPlayerNumber();
	}

	public Vector3 LastDir() {
		return _lastDir;
	}
	
	public void StartParry() {
		if (!IsUnable() && IsGrounded())
			_atks.StartParry();
	}

	public void StopParry() {
		if (_atks.IsParrying())
			_atks.StopParry();
	}

	public bool IsParrying() {
		return _atks.IsParrying();
	}

	public bool Cooldown() {
		return _atks.Cooldown();
	}

	public bool CanCounter() {
		return _atks.CanCounter();
	}
	
	public int GetAttackId() {
		return _atks.GetAttackId();
	}

	//Função de ataque **PODE SER MELHOR**
	public void Attack() {
		if (!IsUnable()) {
			_atks.DoAttack();
		}		
	}

	public void Dash() {
		if (IsUnable() || _dashed)
			return;
		
		if (_dashRoutine != null) {
			StopCoroutine(_dashRoutine);
		}

		_dashRoutine = StartCoroutine(DoDash(_lastDir, 0.1f));
		ScreenEffects.Instance.CreateDashParticles(transform, -_lastDir);
	}

	private IEnumerator DoDash(Vector3 dir, float timeToDash) {
		_dashing = true;
		float timer = 0;
		while (timer < timeToDash) {
			RaycastHit hit;
			
			if (Physics.SphereCast(transform.position + (Vector3.up * _colliderRadiusY), _colliderRadiusX * 0.25f, dir, out hit, _dashSpeed * Time.deltaTime)) {
				transform.position += dir * (hit.distance - _colliderRadiusX);;
				if (hit.collider.CompareTag("Player")) {
					
					var orc = hit.collider.gameObject.GetComponent<FighterBehaviour>();
					if (orc != null) {
						var d = (orc.transform.position - transform.position).normalized;
						_sfx.PlaySFxByIndex(3, Random.Range(0.8f, 1.2f));
						if (orc.IsParrying()) {
							orc.StopParry();
							_sfx.PlaySFxByIndex(2, Random.Range(0.8f, 1.2f));
							orc.Damage(d, 100, 1f, true, true);
							ScreenEffects.Instance.FreezeFrame(0.15f);
						} else {
							ScreenEffects.Instance.FreezeFrame(0.08f);
							orc.Damage(d, 25, 0.25f, true, false);
						}
					}
				}
				break;
			}
			
			transform.position += dir * _dashSpeed * Time.deltaTime;
			
			timer += Time.deltaTime;
			yield return null;
		}
		_dashing = false;
		_dashed = true;
	}

	public bool JumpingInput() {
		return GetComponentInParent<ObsoletePlayerInput>().IsJumping();
	}

	public bool CanStomp() {
		return _canStomp;
	}
	
	public bool IsBurning() {
		return _burning;
	}

	public void Vibrate(int motorIndex, float motorLevel, float duration) {
		_input.Vibrate(motorIndex, motorLevel, duration);
	}

	public void Damage(Vector3 dir, float force, float hurtTime, bool knockBack, bool knockUp) {
		_hurtForSeconds = hurtTime;	
		_hurt = true;
		_taunt = false;
		_canStomp = false;
		_tauntTimer = 0;
		if (!IsParrying()) {
			
			AddDamage((int) ((_damage + force) * 0.07f));
			ScreenEffects.Instance.CreateHitParticles(transform, dir);
			
			FlashColor(_colors.HurtColor, 0.2f, 1f);
			
			if (knockUp)
				dir = (dir + (Vector3.up * 0.5f)).normalized;

			if (knockBack)
				_rb.AddForce(dir * (force + _damage), ForceMode.Impulse);
				
		} else {
			_sfx.PlaySFxByIndex(3, Random.Range(0.8f, 1.2f));
			FlashColor(_colors.HurtColor, 0.2f, 0.6f);
		}		    
		
		Vibrate(0, (force + _damage) * 0.1f , 0.1f);		
		_atks.WasHit(_damage);
	}

	public void FlashColorAfterSeconds(Color c, float wait, float timeToFlash, float intensity) {
		_flash.ImpactFlashAfterSeconds(c, wait, timeToFlash, intensity);
	}
	
	public void FlashColor(Color c1, float timeToFlash, float intensity) {
		_flash.ImpactFlash(c1, timeToFlash, intensity);
	}

	public void SetTintColor(Color c) {
		_flash.SetTintColor(c);
	}
	
}
