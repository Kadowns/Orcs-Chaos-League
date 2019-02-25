using System.Collections;
using System.Collections.Generic;
using OCL;
using UnityEngine;

public class ArenaController : Singleton<ArenaController> {

	public ArenaMotor ArenaMotor { get; set; }
	public List<ArenaState> Arenas;
	public int SelectedArena;
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


	private ArenaState m_currentArena;
	private Coroutine _greatEventRoutine;
    private PlayerController[] _players;
	private int _randomEvent;
	private float _eventTimer;
	private bool _startGame, _gameStarted;	
	private HUDController _hud;
	//private MusicController _music;

	private void Awake() {
		_players = GameController.Instance.PlayerControllers;
		m_currentArena = Arenas[SelectedArena];
		m_currentArena.gameObject.SetActive(true);
		
	}
	
	private void Start() {
		foreach (var ev in m_currentArena.GreatEvents) {
			ev.Setup(m_currentArena);
		}
		
		_hud = HUDController.Instance;
		//_music = MusicController.Instance;
	}

	private void Update() {
		if (ArenaMotor == null)
			return;
		
		ArenaMotor.Tick(this, m_currentArena);
		
		if (_startGame && !_gameStarted) {
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
		_randomEvent = Random.Range(0, m_currentArena.GreatEvents.Count);
		m_currentArena.GreatEvents[_randomEvent].Execute(m_currentArena);
		yield return new WaitForSeconds(m_currentArena.GreatEvents[_randomEvent].Duration);
		m_currentArena.GreatEvents[_randomEvent].Terminate(m_currentArena);
		GreatEventInExecution = false;
		
		_eventTimer = 0;
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
		    m_currentArena.GreatEvents[_randomEvent].Terminate(m_currentArena);
		    GreatEventInExecution = false;
	    }
        Debug.Log("Winner is Player" + winnerNumber);
        GameController.Instance.EndMatch();
	    _eventTimer = 0;
        _gameStarted = false;
	    _startGame = false;
        ArenaMotor.ResetToDefault(this, m_currentArena);
    }

	public void PrepareGame(ref PlayerController[] players) {
       
        PlayerInGame = GetActivePlayers();
		ArenaMotor = m_currentArena.Motor;
		ArenaMotor.Setup(this, m_currentArena);
		ArenaMotor.Initialize(this, m_currentArena);
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
