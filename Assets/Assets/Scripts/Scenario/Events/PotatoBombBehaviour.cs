using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoBombBehaviour : MonoBehaviour, ISpawnable {

	public float TimeToExplode;
	public float MoveSpeed = 100f;
	public float YOffset = 10;
    public float FlashOscilationFrequency;

	[SerializeField] private float _spawnForce;

	private int _lastTargetId = -1;

	private bool _exploding;

	private ScreenEffects _fx;

    private Coroutine _setoffTimerRoutine;

	private Rigidbody _rb;

    private Animator _animator;
	
	private PlayerController _targetController;

    private Transform _targetTransform;

	private void Awake() {
        _animator = GetComponentInChildren<Animator>();
		_rb = GetComponent<Rigidbody>();
	}

	private void Start() {
		_fx = ScreenEffects.Instance;
	}

	private void FixedUpdate() {
		if (_exploding) {
			_rb.MovePosition(Vector3.Lerp(_rb.position, _targetTransform.position, MoveSpeed * Time.deltaTime));		
			return;
		}

		if (_targetTransform != null) {
			if (!_targetTransform.gameObject.activeInHierarchy) {
				_targetController.HitEvent -= ChangeTarget;
				_lastTargetId = -1;
				_targetController = null;
				_targetTransform = null;
				
			} else {
				_rb.MovePosition(Vector3.Lerp(_rb.position, _targetTransform.position + Vector3.up * YOffset,
					MoveSpeed * Time.deltaTime));
				
			}
		}
		else {
			FindClosestOrc();
		}		
	}

	private void OnCollisionEnter(Collision other) {
		if (!_exploding || other.collider.gameObject.layer != LayerMask.NameToLayer("Players"))
			return;
		
		var entity = other.gameObject.GetComponent<MovableEntity>();
		if (entity == null)
			return;
		var motor = entity.Motor as OrcMotor;
		var state = entity.State as OrcEntityState;
		
		motor.Damage(state, (state.transform.position - transform.position).normalized, 300, 1f, true, true, _lastTargetId);
		
		_fx.ScreenShake(0.15f, 1.5f);
		_fx.FreezeFrame(0.1f);
		_fx.CreateGuidedMissileExpParticles(transform.position);
		
		
		gameObject.SetActive(false);
		ResetToDefault();
	}

	public void ResetToDefault() {
		_targetController.HitEvent -= ChangeTarget;
		_targetController = null;
		_targetTransform = null;
		_exploding = false;
		_animator.speed = 1;
		_lastTargetId = -1;	
	}

	public void OnSpawn() {
		_rb.AddForce(Vector3.up * _spawnForce, ForceMode.Impulse);
	}

    public void ChangeTarget(PlayerController other) {
	    if (_exploding)
		    return;
	    
	    _lastTargetId = _targetController.PlayerNumber;
        _targetController.HitEvent -= ChangeTarget;
        _targetController = other;
        _targetTransform = _targetController.Orc.transform;
        _targetController.HitEvent += ChangeTarget;
    }

	public void FindClosestOrc() {
		var cols = Physics.OverlapSphere(transform.position, 500f, 1 << 11);
		float maxDist = float.MaxValue;
		foreach (var col in cols) {
			float dist = (col.transform.position - transform.position).sqrMagnitude;
			if (dist < maxDist) {
				maxDist = dist;
				_targetTransform = col.transform;
			}
		}
        if (_targetTransform != null) {
            _targetController = _targetTransform.GetComponent<OrcEntityState>().Controller;
            _targetController.HitEvent += ChangeTarget;
            if (_setoffTimerRoutine == null)
                _setoffTimerRoutine = StartCoroutine(SetOffTimer());
        }
	}

    private IEnumerator SetOffTimer() {
        
        float timer = 0;
        while (timer < TimeToExplode) {
            if (_targetTransform != null && _targetTransform.gameObject.activeInHierarchy)
                timer += Time.deltaTime;
            _animator.speed = 1 + (timer / TimeToExplode) * FlashOscilationFrequency;
            yield return null;
        }
	    _exploding = true;    
        
    }
}
