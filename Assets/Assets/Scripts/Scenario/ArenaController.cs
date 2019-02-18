using System.Collections;
using System.Collections.Generic;
using OCL;
using UnityEngine;

public class ArenaController : Singleton<ArenaController> {

	public ArenaMotor ArenaMotor { get; set; }
	public ArenaState Arena;
	public int PointsToWin = 10;
	public float EventIntervalTime;
	public int PlayerInGame { get; private set; }

	public bool GreatEventInExecution { get; private set; }

	public bool GameStated {
		get { return _gameStarted; }
	}

	public bool StartGame {
		get { return _startGame; }
	}

	private Coroutine _greatEventRoutine;
    private PlayerController[] _players;
	private int _randomEvent;
	private float _eventTimer;
	private bool _startGame, _gameStarted;	
	private HUDController _hud;
	//private MusicController _music;

	private void Awake() {
		_players = GameController.Instance.PlayerControllers;	
	}
	
	private void Start() {
		foreach (var ev in Arena.GreatEvents) {
			ev.Setup(Arena);
		}
		
		_hud = HUDController.Instance;
		//_music = MusicController.Instance;
	}

	private void Update() {
		if (ArenaMotor == null)
			return;
		
		ArenaMotor.Tick(this, Arena);
		
		if (_startGame && !_gameStarted) {
			ArenaMotor.NormalEvents(this, Arena);
			_gameStarted = true;
			_hud.FighterHud(true);
			GameController.Instance.PlayMainTheme();
			AudioController.Instance.ChangeCutoffFrequency(22000f, 0.25f);
		}

    
		if (_gameStarted) {
			_eventTimer += Time.deltaTime;
			if (!GreatEventInExecution && _eventTimer > EventIntervalTime) {
			
				_greatEventRoutine = StartCoroutine(DoGreatEvent());
			}
		}
	}

	private IEnumerator DoGreatEvent() {
		GreatEventInExecution = true;
		_randomEvent = Random.Range(0, Arena.GreatEvents.Count);
		Arena.GreatEvents[_randomEvent].Execute(Arena);
		yield return new WaitForSeconds(Arena.GreatEvents[_randomEvent].Duration);
		Arena.GreatEvents[_randomEvent].Terminate(Arena);
		GreatEventInExecution = false;
		
		_eventTimer = 0;
		ArenaMotor.NormalEvents(this, Arena);
	}

    public void GameShouldEnd(int winnerNumber) {
        if (winnerNumber == -1) {
            int maxScore = 0;
            foreach (var p in _players) {
                if (!p.gameObject.activeInHierarchy)
                    continue;

	            int score = p.KillCount;
	            if (score > maxScore) {
		            maxScore = score;
		            winnerNumber = p.PlayerNumber;
	            }
            }
        }

	    if (_greatEventRoutine != null) {
		    StopCoroutine(_greatEventRoutine);    
		    Arena.GreatEvents[_randomEvent].Terminate(Arena);
		    GreatEventInExecution = false;
	    }
        Debug.Log("Winner is Player" + winnerNumber);
        GameController.Instance.EndMatch();
	    _eventTimer = 0;
        _gameStarted = false;
	    _startGame = false;
        ArenaMotor.ResetToDefault(this, Arena);
    }

	public void PrepareGame(ref PlayerController[] players) {
       
        PlayerInGame = GetActivePlayers();
		ArenaMotor = Arena.Motor;
		ArenaMotor.Setup(this, Arena);
		ArenaMotor.Initialize(this, Arena);
	}

	public void PlayerSpawned() {
		_startGame = true;
	}

    public int GetActivePlayers() {
        int sum = 0;
        foreach (var p in _players) {
            if (!p.gameObject.activeInHierarchy)
                continue;

            sum++;
        }
        return sum;
    }
}
