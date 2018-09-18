using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : Singleton<ArenaController> {

	public ArenaMotor ArenaMotor { get; set; }
	public ArenaState[] Arenas;
    public float TimeLimit = 180;
	public float SuddenDeathWait;
	public int PlayerInGame;

	public bool SuddenDeath {
		get { return _suddenDeath; }
	}

	public bool GameStated {
		get { return _gameStarted; }
	}

	public bool StartGame {
		get { return _startGame; }
	}

	public int CurrentArena {
		get { return _currentArena; }
	}

    private PlayerController[] _players;
	private int _currentArena;
	private float _suddenDeathTimer, _gameTimer;
	private bool _startGame, _gameStarted, _suddenDeath;	
	private HUDController _hud;
	private MusicController _music;
	private ScreenEffects _fx;

	private void Start() {
		_hud = HUDController.Instance;
		_music = MusicController.Instance;
		_fx = ScreenEffects.Instance;

		_players = GameController.Instance.PlayerControllers;
		foreach (var p in _players) {
			p.DeathEvent += PlayerDied;
		}
	}

	private void Update() {
		if (ArenaMotor == null)
			return;
		
		ArenaMotor.Tick(this, Arenas[_currentArena]);
		
		if (_startGame && !_gameStarted) {
			ArenaMotor.NormalEvents(this, Arenas[_currentArena]);
			_gameStarted = true;
			_fx.SetCameraAnimationTrigger("StopFx");
			_hud.FighterHud(true);
			_music.PlayBgmByIndex(1);
			_music.SetBGMPitch(1);
			_music.ChangeLowPassFilterFrequency(22000f, 0.25f);
		}

    
		if (_gameStarted) {
            _gameTimer += Time.deltaTime;
			if (!_suddenDeath && _gameTimer > SuddenDeathWait) {
				_fx.ScreenShake(1.5f, 5f);
				_suddenDeath = true;
				ArenaMotor.OnSuddenDeath(this, Arenas[_currentArena]);
				_fx.BlurForSeconds(1.5f, 20f, new Color(0.85f, 0.85f, 0.85f), new Color(1f, 0.85f, 0.85f));
				_music.PlayBgmByIndex(1);
				_music.SetBGMPitch(1.075f);
				_music.DramaticFrequencyChange(0.1f, 1.5f, 0.5f, 200f, 22000f);
			}
            else if (_gameTimer > TimeLimit) {
                GameShouldEnd(-1);
            }
		}
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
        Debug.Log("Winner is Player" + winnerNumber);
        GameController.Instance.SetGameState(-1);
        _suddenDeath = false;
        _gameTimer = 0;
        _gameStarted = false;
	    _startGame = false;
        ArenaMotor.ResetToDefault(this, Arenas[_currentArena]);
    }

	public void PlayerDied(int attackerNumber) {
		if (_players == null || attackerNumber == -1)
			return;
		
		_players[attackerNumber].GotKill();
		
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
