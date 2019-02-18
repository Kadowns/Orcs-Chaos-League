using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PlayerControllers.Input;
using Assets.Scripts.PlayerControllers.Orc.PowerUps;
using OCL;
using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public event Action OnDamage;
	public event Action<int> OnDeath;
	public event Action<int> OnSkullCollected;
    public event Action<PlayerController> OnHit;

	[SerializeField] private int _playerNumber;

	public int PlayerNumber {
		get { return _playerNumber; }
	}
	
	public int OrcDamage {
		get { return (_orcState != null ? _orcState.Damage : 0); }
	}

	public bool AutoSpawn;

	public bool GivePowerUp;

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

	private InputController _input;

	private Player _player;

	private List<PowerUp> m_powerUps = new List<PowerUp>();
	
	public int KillCount { get; private set; }
	public int LastAttackerNumber;
	private int _actualGameState;
	
    private float _spawnTimer = 0, _timeToGiveKill = 0;

	private bool _spawnNewOrc;

	private void Awake() {
		
		_pointer = GetComponent<PlayerPointer>();
		_input = GetComponent<InputController>();
			
		
		CreateOrc();
		BindBox();
	}
	
	private void Start() {
		_pool = ObjectPooler.Instance;
		
		if (PlayerData.CPU[_playerNumber]) {

			BotBrains.BotBrainMemories[_playerNumber].ThisOrc = _orc.transform;
			BotBrains.BotBrainMemories[_playerNumber].ThisBox = _box.transform;
			_input.IsBot(true);
		}
		else {

			_player = ReInput.players.GetPlayer(_playerNumber);
			PlayerBrains.BrainMemory[_playerNumber].Player = _player;
			_input.IsBot(false);
		}
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
		if (GivePowerUp) {
			AddPowerUp(new SpeedModifier(GameConstants.SPEED_INCREASE));
			AddPowerUp(new Shield());
			AddPowerUp(new ExtraJump());
			AddPowerUp(new Frenzy());
			AddPowerUp(new StompModifier(GameConstants.STOMP_INCREASE));
			AddPowerUp(new StompModifier(GameConstants.STOMP_INCREASE));
			AddPowerUp(new StompModifier(GameConstants.STOMP_INCREASE));
		}
	}

	public void AddPowerUp(PowerUp power) {
		power.Initialize(_orcState);
		m_powerUps.Add(power);
	}

	public void RemovePowerUp<T>() where T : PowerUp {
		Type type = typeof(T);		 
		var power = m_powerUps.Find(p => type == p.GetType());
		if (power == null) 
			return;	
			
		power.Terminate(_orcState);
		m_powerUps.Remove(power);
	}

	private void BindBox() {
				
		_box.Input = _input;
		_boxMotor = _box.Motor as BoxMotor;
		_boxState = _box.State as BoxEntityState;
		_boxState.Controller = this;
	}

    public void Hit(PlayerController other) {
        if (OnHit != null) {
            OnHit.Invoke(other);
        }
    }

	public void SpawnOrc(Vector3 position) {

		_orcPrefab.SetActive(true);

		ArenaController.Instance.PlayerSpawned();
		
		_spawnTimer = 0;
		_boxState.CanSpawn = false;
		_orcPrefab.transform.rotation = Quaternion.identity;
		_orcPrefab.transform.position = position;
		_orcPrefab.transform.parent = transform;
		_orcMotor.ResetToDefault(_orcState);
		SetPointerTarget(_orcPrefab.transform);
		_spawnNewOrc = false;


		if (OnDamage != null)
			OnDamage();
		CameraController.Instance.UpdatePlayers();
		//CameraController.Instance.MaxZoom(false);
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
		if (OnDamage != null)
			OnDamage();
		LastAttackerNumber = attackerNumber;
		_timeToGiveKill = Time.time + _attackerShouldReceiveKillTime;
	}

	public void GotKill() {
		if (!_orc.gameObject.activeInHierarchy)
			return;
		KillCount++;
		_orcState.Damage = _orcState.Damage - 15 < 0 ? 0 : _orcState.Damage - 15;			
		if (OnDamage != null)
			OnDamage.Invoke();
		ScreenEffects.Instance.CreatePlusOneParticles(_orc.transform.position + Vector3.up * 6);
		Vibrate(1, 0.2f, 0.15f);
		if (OnSkullCollected != null)
			OnSkullCollected.Invoke(KillCount);
		if (KillCount >= ArenaController.Instance.PointsToWin) {
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

		if (OnDamage != null)
			OnDamage.Invoke();
		if (OnSkullCollected != null)
			OnSkullCollected.Invoke(KillCount);
		if (OnDeath != null)
			OnDeath.Invoke(LastAttackerNumber);
	}

	private void Update() {
		if (_actualGameState != 1)
			return;
		if (_input.CenterButtonPresssed) {
			//GameController.Instance.Pause(PlayerNumber);
		}

		if (LastAttackerNumber != -1 && Time.time > _timeToGiveKill) {
			LastAttackerNumber = -1;
		}

		if (_spawnNewOrc) {
			if (Time.time > _spawnTimer + _maxTimeToSpawn) {
				_boxMotor.ReleaseBox(_boxState);
			}
			else if (Time.time > _spawnTimer) {
				_boxState.CanSpawn = true;
			}			
		}
		
		m_powerUps.ForEach(p => p.Tick(_orcState));
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
		//CameraController.Instance.MaxZoom(true);
	}

	public void SetDefaultPosition(Vector3 position) {
		_boxMotor.SetDefaultRaised(_boxState, position.y + 150);
		_boxMotor.SetDefaultLowered(_boxState, position.y + 100);
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
