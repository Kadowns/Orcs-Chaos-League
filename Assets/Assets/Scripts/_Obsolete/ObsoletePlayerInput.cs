using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using Rewired;
using Rewired.Data.Mapping;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

public class ObsoletePlayerInput : MonoBehaviour {


	public bool AutoSpawn;

	[SerializeField] private int _playerNumber;
	[SerializeField] private FighterHUD _hud;
	[SerializeField] private float _maxTimeToSpawn;
    

    private float _spawnTimer;

    private Player _player;
	
	private GameObject _orc;

	private FighterBehaviour _fighter;
	
	private SpawnerBehaviour _spawner;

	private bool _jumping, _moving, _spawnedAtHub;


	private void Awake() {

		_spawner = GetComponent<SpawnerBehaviour>();
		_player = ReInput.players.GetPlayer(_playerNumber);
		_orc = _spawner.CreateOrc();
		_fighter = _orc.GetComponent<FighterBehaviour>();
	}
	
	
	private void Update () {
		
		//Variaveis pra pegar o valor do input de movimento
		float vert = _player.GetAxisRaw("Vertical");
		float hori = _player.GetAxisRaw("Horizontal");
		
		if (_orc.activeInHierarchy) {
			
			//Input de movimento------------
			//Vertical
			if (Mathf.Abs(vert) > 0f || Mathf.Abs(hori) > 0f) {
				_fighter.Move(new Vector3(hori, 0, vert).normalized);
				_moving = true;
				
			} else {
				_moving = false;
			}
			
			if (_player.GetButtonDown("Dash")) {
				_fighter.Dash();
			}
		
			//-----------------------------

			//Input de pulo----------------
			if (_player.GetButtonDown("Jump")) {
				_fighter.Jump();
				_jumping = true;
			} else if (_player.GetButtonUp("Jump")) {
				_jumping = false;
			}
			//-----------------------------

			//Input de ataque--------------
			if (_player.GetButtonDown("Attack")) {
				_fighter.Attack();
			}
			else if (_player.GetButtonUp("Attack")) {
				
			}
			
			if (_player.GetButtonDown("Parry")) {
				_fighter.StartParry();
			}
			else if (_player.GetButtonUp("Parry")) {
				_fighter.StopParry();
			}
			
			//Input de Taunt
			if (_player.GetButtonDown("Taunt1")) {
				_fighter.Taunt();
			}

			if (_player.GetButtonDown("Pause")) {
				GameController.Instance.Pause(_playerNumber);               
            }
			

		} else if (GameController.Instance.GetGameState() == 1) {
			if (Mathf.Abs(vert) >= 0.01f || Mathf.Abs(hori) >= 0.01f) {
				_spawner.Move(new Vector3(hori, 0, vert).normalized);
			}

			_spawnTimer += Time.deltaTime;
			if ((ActionButton() || _maxTimeToSpawn < _spawnTimer) && _spawner.CanSpawn()) {
				_spawnTimer = 0;
				CreateNewOrc();		
			}
		}
		else if (GameController.Instance.GetGameState() == 0){
			if ((_player.GetAnyButton() || AutoSpawn) && !_spawnedAtHub) {
				_hud.gameObject.SetActive(true);
				CreateNewOrc();
				_spawnedAtHub = true;
			}
			else if (_spawnedAtHub && ActionButton()) {
				_spawner.PlayerIsNotReady();
			}
		}
		else {
			if (ActionButton()) {
				GameController.Instance.StartGame();
			}
		}
	}

	private bool ActionButton() {
		if (_player.GetButtonDown("Jump") ||
		    _player.GetButtonDown("Attack") ||
		    _player.GetButtonDown("Parry") ||
		    _player.GetButtonDown("Dash"))
			return true;

		return false;
	}
	
	public void Vibrate(int motorIndex, float motorLevel, float duration) {
		_player.SetVibration(motorIndex, motorLevel, duration);
	}

	public void ChangeInputMode(string categoryToDisable, string categoryToEnable) {
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryToDisable)) {
			controllerMap.enabled = false;
		}
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryToEnable)) {
			controllerMap.enabled = true;			
		}
	}

	public void SetInputMode(string categoryName, bool value) {
		foreach (var controllerMap in _player.controllers.maps.GetAllMapsInCategory(categoryName)) {
			controllerMap.enabled = value;
		}
	}

	private void CreateNewOrc() {

		if (GameController.Instance.GetGameState() == 0 && !_spawnedAtHub) {
			_orc = _spawner.SpawnOrcAt(new Vector3(0, 510, 0), transform);	
		}
		else {
			_orc = _spawner.SpawnNewOrc();
		}	
	}

	public void ResetToDefault() {
		
		_hud.gameObject.SetActive(false);
		_spawnedAtHub = false;
		_spawnTimer = 0;
		SetPointerTarget(null);
	}
	
	private void CreateNewOrcAt(Vector3 target) {
		
		_orc = _spawner.SpawnOrcAt(target, transform);		
		CameraController.Instance.UpdatePlayers();		
	}

	public void SetPointerTarget(Transform target) {
		GetComponent<PlayerPointer>().SetTarget(target);
	}
	
	public void UpdateDamage(int damage) {
		_hud.UpdateScore(damage);
	}

	public void SubtractLife() {

		_spawner.SubtractLife();
		_hud.UpdateScore("---");
		var lifes = _spawner.GetOrcs();
		if (lifes >= 0)
			_hud.UpdateLife(lifes);
		else
			_hud.UpdateLife(0);
	}

	public int GetPlayerNumber() {
		return _playerNumber;
	}

	public bool IsMoving() {
		return _moving;
	}

	public bool IsJumping() {
		return _jumping;
	}
}
