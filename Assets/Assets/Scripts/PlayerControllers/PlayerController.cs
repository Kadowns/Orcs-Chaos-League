using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public delegate void ScoreDelegate();
	public event ScoreDelegate DamageEvent;

	public delegate void DeathDelegate(int attackerNumber);
	public event DeathDelegate DeathEvent;

	public delegate void KilledDelegate(int count);
	public event KilledDelegate KilledEvent;

    public delegate void HitDelegate(PlayerController other);
    public event HitDelegate HitEvent;

	[SerializeField] private int _playerNumber;

	public int PlayerNumber {
		get { return _playerNumber; }
	}
	
	public int OrcDamage {
		get { return (_orcState != null ? _orcState.Damage : 0); }
	}

	public bool AutoSpawn;

    [SerializeField] GameObject _orcPrefab;
	[SerializeField] private MovableEntity _box;
	[SerializeField] private FighterHUD _hud;
    [SerializeField] private float _spawnWait = 3;
	[SerializeField] private float _attackerShouldReceiveKillTime = 5f;
	[SerializeField] private float _maxTimeToSpawn;

	public FighterHUD Hud {
		get { return _hud; }
	}

    public MovableEntity Orc {
        get {
            return _orc;
        }
    }

	private PlayerPointer _pointer;
	private MovableEntity _orc;
	private BoxMotor _boxMotor;
	private OrcMotor _orcMotor;
	private BoxEntityState _boxState;
	private OrcEntityState _orcState;
	private ObjectPooler _pool;

	private InputSource _input;

	private Player _player;
	
	public int KillCount { get; private set; }
	public int LastAttackerNumber;
	private int _actualGameState;
	
    private float _spawnTimer = 0, _timeToGiveKill = 0;

	private bool _spawnNewOrc, _playerInGame;

	public bool PlayerInGame {
		get { return _playerInGame; }
	}

	private void Awake() {
		
		_pointer = GetComponent<PlayerPointer>();
		_input = GetComponent<InputSource>();
			
		
		CreateOrc();
		BindBox();
		
		var playerInput = _input as PlayerInput;
		if (playerInput != null) {
			_player = ReInput.players.GetPlayer(_playerNumber);
			playerInput.Player = _player;
		}
		else {
			var botInput = _input as BotInput;
			if (botInput != null) {
				botInput.ThisOrc = _orc.transform;
				botInput.ThisBox = _box.transform;
			}
		}
	}
	
	private void Start() {
		_pool = ObjectPooler.Instance;
		
	}

	private void CreateOrc() {
        _orcPrefab = Instantiate(_orcPrefab, transform);
        _orcPrefab.SetActive(false);
		_orc = _orcPrefab.GetComponent<MovableEntity>();
		_orc.Input = _input;
		_orcMotor = _orc.Motor as OrcMotor;
		_orcState = _orc.State as OrcEntityState;
		_orcState.Controller = this;
		_orcState.PlayerColor = _pointer.GetPlayerColor();

	}

	private void BindBox() {
				
		_box.Input = _input;
		_boxMotor = _box.Motor as BoxMotor;
		_boxState = _box.State as BoxEntityState;
		_boxState.Controller = this;
	}

    public void Hit(PlayerController other) {
        if (HitEvent != null) {
            HitEvent.Invoke(other);
        }
    }

	public void SpawnOrc() {

		_orcPrefab.SetActive(true);

		ArenaController.Instance.PlayerSpawned();
		_boxMotor.DisableBox(_boxState);
		_boxMotor.RaiseBox(_boxState);
		_boxState.CanSpawn = false;
		_orcPrefab.GetComponent<Rigidbody>().AddForce(_box.GetComponent<Rigidbody>().velocity * 4f, ForceMode.Impulse);

		_spawnTimer = 0;
		_orcPrefab.transform.rotation = Quaternion.identity;
		_orcPrefab.transform.position = _box.transform.position;
		_orcPrefab.transform.parent = transform;
		_orcMotor.ResetToDefault(_orcState);
		SetPointerTarget(_orcPrefab.transform);
		_spawnNewOrc = false;


		DamageEvent.Invoke();
		CameraController.Instance.UpdatePlayers();
		CameraController.Instance.MaxZoom(false);
	}

	public void Vibrate(int motorIndex, float motorLevel, float duration) {
		if (_player == null)
			return;
		
		_player.SetVibration(motorIndex, motorLevel, duration);
	}

	public void SetPointerTarget(Transform target) {
		_pointer.SetTarget(target);
	}

	public void WasHit(int attackerNumber) {
		DamageEvent.Invoke();
		LastAttackerNumber = attackerNumber;
		_timeToGiveKill = Time.time + _attackerShouldReceiveKillTime;
	}

	public void GotKill() {
		if (!_orc.gameObject.activeInHierarchy)
			return;
		KillCount++;
		ScreenEffects.Instance.CreatePlusOneParticles(_orc.transform.position + Vector3.up * 6);
		Vibrate(1, 0.2f, 0.15f);
		if (KilledEvent != null)
			KilledEvent.Invoke(KillCount);
		if (KillCount >= 10) {
			ArenaController.Instance.GameShouldEnd(_playerNumber);
		}	
	}

	public void SubtractLife() {

		for (int i = 0; i < KillCount + 1; i++) {
			_pool.SpawnFromPool("OrcHead", _orc.transform.position, Quaternion.identity);
		}

		KillCount = 0;	
		SetPointerTarget(_box.transform);
		StartSpawning();

		if (DamageEvent != null)
			DamageEvent.Invoke();
		if (KilledEvent != null)
			KilledEvent.Invoke(KillCount);
		if (DeathEvent != null)
			DeathEvent.Invoke(LastAttackerNumber);
	}

	private void Update() {
		if (_actualGameState != 1)
			return;
		if (_input.CenterButtonPresssed) {
			GameController.Instance.Pause(PlayerNumber);
		}

		if (LastAttackerNumber != -1 && Time.time > _timeToGiveKill) {
			LastAttackerNumber = -1;
		}

		if (_spawnNewOrc) {
			CameraController.Instance.MaxZoom(true);
			if (Time.time > _spawnTimer) {
				_boxState.CanSpawn = true;
				if (Time.time > _spawnTimer + _maxTimeToSpawn) {
					SpawnOrc();
				}
			}
		}
	}

	public void UpdateGameState(int index) {
		_actualGameState = index;
	}

	public void ReadyToSpawn() {
		_spawnNewOrc = true;
		_spawnTimer = Time.time + _spawnWait;
	}

	public void ResetToDefault() {		
		KillCount = 0;
		_spawnTimer = 0;
		_spawnNewOrc = false;
		_hud.ResetToDefault();
		_boxMotor.ResetToDefault(_boxState);
	}

	public void StartSpawning() {	
		_boxMotor.Free(_boxState);
		_boxMotor.EnableBox(_boxState);
		_boxMotor.LowerBox(_boxState);
		SetPointerTarget(_box.transform);
		CameraController.Instance.MaxZoom(true);
	}

	public void SetDefaultPosition(Vector3 position) {
		_boxMotor.SetDefaultRaised(_boxState, position.y + 300);
		_boxMotor.SetDefaultLowered(_boxState, position.y + 200);
	}
	
	public void ChangeInputMode(string categoryToDisable, string categoryToEnable) {

		if (_player == null)
			return;
		
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryToDisable)) {
			controllerMap.enabled = false;
		}
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryToEnable)) {
			controllerMap.enabled = true;			
		}
	}

	public void SetInputMode(string categoryName, bool value) {
		if (_player == null)
			return;
		
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryName)) {
			controllerMap.enabled = value;
		}
	}
}
