using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : Singleton<ArenaController> {

	public ArenaMotor ArenaMotor { get; set; }
	public ArenaState[] Arenas;
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

	public int CurrentArena {
		get { return _currentArena; }
	}

	private Coroutine _greatEventRoutine;
    private PlayerController[] _players;
	private int _currentArena, _randomEvent;
	private float _gameTimer, _eventTimer;
	private bool _startGame, _gameStarted;	
	private HUDController _hud;
	private MusicController _music;
	private ScreenEffects _fx;

	private void Awake() {
		_players = GameController.Instance.PlayerControllers;	
	}
	
	private void Start() {
		foreach (var ev in Arenas[_currentArena].GreatEvents) {
			ev.Setup(Arenas[_currentArena]);
		}
		
		_hud = HUDController.Instance;
		_music = MusicController.Instance;
		_fx = ScreenEffects.Instance;	
	}

	private void Update() {
		if (ArenaMotor == null)
			return;
		
		ArenaMotor.Tick(this, Arenas[_currentArena]);
		
		if (_startGame && !_gameStarted) {
			ArenaMotor.NormalEvents(this, Arenas[_currentArena]);
			_gameStarted = true;
			_hud.FighterHud(true);
			_music.PlayBgmByIndex(0);
			_music.SetBGMPitch(1);
			_music.ChangeLowPassFilterFrequency(22000f, 0.25f);
		}

    
		if (_gameStarted) {
			_eventTimer += Time.deltaTime;
            _gameTimer += Time.deltaTime;
			if (!GreatEventInExecution && _eventTimer > EventIntervalTime) {
			
				_greatEventRoutine = StartCoroutine(DoGreatEvent());
			}
		}
	}

	private IEnumerator DoGreatEvent() {
		GreatEventInExecution = true;
		_randomEvent = Random.Range(0, Arenas[_currentArena].GreatEvents.Count);
		Arenas[_currentArena].GreatEvents[_randomEvent].Execute(Arenas[_currentArena]);
		yield return new WaitForSeconds(Arenas[_currentArena].GreatEvents[_randomEvent].Duration);
		Arenas[_currentArena].GreatEvents[_randomEvent].Terminate(Arenas[_currentArena]);
		GreatEventInExecution = false;
		
		_eventTimer = 0;
		ArenaMotor.NormalEvents(this, Arenas[_currentArena]);
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
		    Arenas[_currentArena].GreatEvents[_randomEvent].Terminate(Arenas[_currentArena]);
		    GreatEventInExecution = false;
	    }
        Debug.Log("Winner is Player" + winnerNumber);
        GameController.Instance.EndMatch();
        _gameTimer = 0;
	    _eventTimer = 0;
        _gameStarted = false;
	    _startGame = false;
        ArenaMotor.ResetToDefault(this, Arenas[_currentArena]);
    }

	public void PrepareGame(ref PlayerController[] players, int currentArena) {
       
        PlayerInGame = GetActivePlayers();
		_currentArena = currentArena;
		ArenaMotor = Arenas[currentArena].Motor;
		ArenaMotor.Setup(this, Arenas[currentArena]);
		ArenaMotor.Initialize(this, Arenas[currentArena]);
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
