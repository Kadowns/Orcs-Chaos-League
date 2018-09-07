using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour {


	[SerializeField] private GameObject _orc;
    [SerializeField] private int _orcs = 3;
    [SerializeField] private float _spawnWait = 3;

	private BoxBehaviour _box;

	private ObsoletePlayerInput _input;
	
    private float _spawnTimer = 0;

	private bool _spawnNewOrc;

	private void Start() {
		_input = GetComponent<ObsoletePlayerInput>();
	}


	public GameObject CreateOrc() {
        _orc = Instantiate(_orc, transform);
        _orc.SetActive(false);
	    
        return _orc;
    }

    private void Update() {
        if (_spawnNewOrc) {
	        CameraController.Instance.MaxZoom(true);
            _spawnTimer += Time.deltaTime;
        }
    }

	public GameObject SpawnOrcAt(Vector3 position, Transform parent) {
        _orc.SetActive(true);
        _orc.transform.rotation = Quaternion.identity;
        _orc.transform.position = position;
        _orc.transform.parent = parent;
        GameController.Instance.IncreaseActivePlayers();
		var fighter = _orc.GetComponent<FighterBehaviour>();
		fighter.ResetToDefault();
		_input.SetPointerTarget(_orc.transform);
		CameraController.Instance.UpdatePlayers();	
        return _orc;
    }
    

    public GameObject SpawnNewOrc() {

	    Vector3 spawnPos;
	    _orc.SetActive(true);
	    if (_box != null) {
		    ArenaController.Instance.PlayerSpawned();
		    _box.DisableBox();
		    _box.RaiseBox();
		    spawnPos = _box.transform.position;
		    _orc.GetComponent<Rigidbody>().AddForce(_box.GetComponent<Rigidbody>().velocity * 4f, ForceMode.Impulse);
	    }
	    else {
		    spawnPos = transform.position;
		    GameController.Instance.IncreaseActivePlayers();
	    }

	    
        _spawnTimer = 0;
        _orc.transform.rotation = Quaternion.identity;
        _orc.transform.position = spawnPos;
        _orc.transform.parent = transform;
	    var fighter = _orc.GetComponent<FighterBehaviour>();
	    fighter.ResetToDefault();
	    _input.SetPointerTarget(_orc.transform);
	    _input.UpdateDamage(fighter.GetDamage());
	    _spawnNewOrc = false;
	    CameraController.Instance.UpdatePlayers();	
	    CameraController.Instance.MaxZoom(false);   
        return _orc;
    }

	public void Move(Vector3 dir) {
		if (_box != null && _spawnNewOrc) {	
			_box.Move(dir);
		}
	}

	public void ReadyToSpawn() {
		_spawnNewOrc = true;
	}

	public void SetDefaultSpawner(GameObject parent) {
		_box = parent.GetComponent<BoxBehaviour>();
		_input.SetPointerTarget(_box.transform);
	}

    public void SubtractLife() {
	    if (_box != null) {
		   
		    _orcs--;
		    if (_orcs < 0) {
			    gameObject.SetActive(false);
			    _input.SetPointerTarget(null);
			    ArenaController.Instance.PlayerDied(_input.GetPlayerNumber());
			    return;
		    }

		    _input.SetPointerTarget(_box.transform);
		    CameraController.Instance.MaxZoom(true);
		    StartSpawning();
	    }
	    else {
		    GetComponent<ObsoletePlayerInput>().ResetToDefault();
		    GameController.Instance.DecreaseActivePlayers();
	    }
    }

	public void ResetToDefault(bool rematch) {
		_orcs = 3;
		if (rematch) 
			return;


		_spawnNewOrc = false;
		CameraController.Instance.MaxZoom(false);
		_input.ResetToDefault();
		if (_box != null)
			_box.ResetToDefault();
		_box = null;
	}

	public void PlayerIsNotReady() {
		_orc.transform.position = _box.transform.position + Vector3.up * 8;
		if (_box != null)
			_box.ResetToDefault();
		_box = null;
		_orc.GetComponent<FighterBehaviour>().ExitBox();
		_input.SetPointerTarget(_orc.transform);
		ScreenEffects.Instance.CreateCageParticles(_orc.transform.position);
		GameController.Instance.DecreaseReadyPlayers();
	}


	public void StartSpawning() {	
		_box.Free();
		_box.EnableBox();
		_box.LowerBox();
		_input.SetPointerTarget(_box.transform);
	}

	public void SetDefaultPosition(Vector3 position) {
		_box.SetDefaultRaised(position.y + 300);
		_box.SetDefaultLowered(position.y + 200);
	}

	public bool PlayerInGame() {
		return _box != null;
	}
	
	public int GetOrcs() {
		return _orcs;
	}
	
	public bool CanSpawn() {
		return _spawnTimer > _spawnWait;
	}

}
