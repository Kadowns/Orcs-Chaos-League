using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public delegate void ScoreDelegate();
	public event ScoreDelegate ScoreEvent;

	public delegate void DeathDelegate(int attackerNumber);
	public event DeathDelegate DeathEvent;

	public delegate void KilledDelegate(int count);
	public event KilledDelegate KilledEvent;

	[SerializeField] private int _playerNumber;

	public int PlayerNumber {
		get { return _playerNumber; }
	}

	public bool AutoSpawn;

    [SerializeField] GameObject _orcPrefab;
	[SerializeField] private FighterHUD _hud;
    [SerializeField] private float _spawnWait = 3;
	[SerializeField] private float _attackerShouldReceiveKillTime = 5f;
	[SerializeField] private float _maxTimeToSpawn;

	private PlayerPointer _pointer;
	private MovableEntity _box, _orc;
	private BoxMotor _boxMotor;
	private OrcMotor _orcMotor;
	private BoxEntityState _boxState;
	private OrcEntityState _orcState;

	private InputSource _input;

	private Player _player;
	
	public int KillCount { get; private set; }
    public int Score { get { return _score; } }
	private int _lastAttackerNumber;
	private int _score = 0;
	private int _actualGameState;
	
    private float _spawnTimer = 0, _timeToGiveKill = 0;

	private bool _spawnNewOrc, _playerInGame;

	public bool PlayerInGame {
		get { return _playerInGame; }
	}

	private void Awake() {
 
		_pointer = GetComponent<PlayerPointer>();
		_input = GetComponent<InputSource>();
		var playerInput = _input as PlayerInput;
		if (playerInput != null) {		
			_player = ReInput.players.GetPlayer(_playerNumber);
			playerInput.Player = _player;
		}			
	}
	
	private void Start() {
		CreateOrc();
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
	
	public void SpawnOrc() {

		_orcPrefab.SetActive(true);
		if (_actualGameState == 0 && !_playerInGame) {
			
			_orcPrefab.transform.rotation = Quaternion.identity;
			_orcPrefab.transform.position = new Vector3(0, 510, 0);
			_orcPrefab.transform.parent = transform;
			_hud.gameObject.SetActive(true);
			GameController.Instance.IncreaseActivePlayers();
			_orcMotor.ResetToDefault(_orcState);
			SetPointerTarget(_orcPrefab.transform);
			_playerInGame = true;

		}
		else {
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

		}
        ScoreEvent.Invoke();
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
		_lastAttackerNumber = attackerNumber;
		_timeToGiveKill = Time.time + _attackerShouldReceiveKillTime;
	}

	public void GotKill() {
		KillCount++;
		KilledEvent.Invoke(KillCount);
	}

	public void SubtractLife() {
		
		if (_box != null) {

            _score /= 2;
            ScoreEvent.Invoke();
		    SetPointerTarget(_box.transform);
			StartSpawning();
		}
		else {
			ResetToDefault(false);
			GameController.Instance.DecreaseActivePlayers();
		}		
		DeathEvent.Invoke(_lastAttackerNumber);
	}

    private void Update() {
	    if (_input.CenterButtonPresssed && _actualGameState != -1) {
		    GameController.Instance.Pause(PlayerNumber);
	    }
	    switch (_actualGameState) {
		    case -1:
                break;
		    case 0:
			    if ((_player.GetAnyButtonDown() || AutoSpawn) && !_playerInGame) {
				    SpawnOrc();			    
			    }
			    break;
		    case 1:
			    if (_lastAttackerNumber != -1 && Time.time > _timeToGiveKill) {
				    _lastAttackerNumber = -1;
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
			    break;
		    
		   default:	
			   if (_player.GetAnyButton()) {
				   GameController.Instance.StartGame();
			   }
			   break;	         	    
	    }	    
    }	
	
	public void AddScore(int scoreToAdd) {
		_score += scoreToAdd;
		ScoreEvent.Invoke();
        if (_score >= 999) {
            ArenaController.Instance.GameShouldEnd(_playerNumber);
        }
	}

	public void UpdateGameState(int index) {
		_actualGameState = index;
	}

	public void ReadyToSpawn() {
		_spawnNewOrc = true;
		_spawnTimer = Time.time + _spawnWait;
	}

	public void SetDefaultSpawner(GameObject parent) {
		_box = parent.GetComponent<MovableEntity>();

		_box.Input = _input;
		_boxMotor = _box.Motor as BoxMotor;
		_boxState = _box.State as BoxEntityState;
		
		SetPointerTarget(_box.transform);
	}

	public void ResetToDefault(bool rematch) {		
        _score = 0;
		KillCount = 0;
		_hud.ResetToDefault();
        if (rematch)   
            return;
        		
		_spawnNewOrc = false;
		CameraController.Instance.MaxZoom(false);
		if (_box != null)
			_boxMotor.ResetToDefault(_boxState);
		_box = null;
		
		_hud.gameObject.SetActive(false);
		_playerInGame= false;
		_spawnTimer = 0;
		SetPointerTarget(null);
	}

	public void PlayerIsNotReady() {
		_orcMotor.ExitBox(_orcState, _box.transform.position);
		if (_box != null)
			_boxMotor.ResetToDefault(_boxState);
		_box = null;
		SetPointerTarget(_orcPrefab.transform);
		ScreenEffects.Instance.CreateCageParticles(_orcPrefab.transform.position);
		GameController.Instance.DecreaseReadyPlayers();
		CameraController.Instance.UpdatePlayers();	
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
	
	public bool CanSpawn() {
		return _spawnTimer > _spawnWait;
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
