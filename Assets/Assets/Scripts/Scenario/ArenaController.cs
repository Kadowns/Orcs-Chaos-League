using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : Singleton<ArenaController> {

	public ArenaMotor ArenaMotor { get; set; }
	public ArenaState[] Arenas;
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

	private int _currentArena;
	private float _suddenDeathTimer;
	private bool _startGame, _gameStarted, _suddenDeath;	
	private HUDController _hud;
	private MusicController _music;
	private ScreenEffects _fx;

	private void Start() {
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
			_fx.SetCameraAnimationTrigger("StopFx");
			_hud.FighterHud(true);
			_music.PlayBgmByIndex(1);
			_music.SetBGMPitch(1);
			_music.ChangeLowPassFilterFrequency(22000f, 0.25f);
		}
	
		if (!_suddenDeath && _gameStarted) {
			_suddenDeathTimer += Time.deltaTime;
			if (_suddenDeathTimer > SuddenDeathWait) {
				_fx.ScreenShake(1.5f, 5f);
				_suddenDeath = true;
				ArenaMotor.OnSuddenDeath(this, Arenas[_currentArena]);
				_fx.BlurForSeconds(1.5f, 20f, new Color(0.85f, 0.85f, 0.85f), new Color(1f, 0.85f, 0.85f));
				_music.PlayBgmByIndex(1);
				_music.SetBGMPitch(1.075f);
				_music.DramaticFrequencyChange(0.1f, 1.5f, 0.5f, 200f, 22000f);
				
				_suddenDeathTimer = 0;
			}
		}
	}

	public void PlayerDied(int playerNumber) {
		PlayerInGame--;
		if (PlayerInGame == 1) {
			GameController.Instance.SetGameState(-1);

			_suddenDeath = false;
			_suddenDeathTimer = 0;
			_gameStarted = false;
			_startGame = false;

			ArenaMotor.ResetToDefault(this, Arenas[_currentArena]);
		}
	}

	public void PrepareGame(int numberOfPlayers, int currentArena) {
		PlayerInGame = numberOfPlayers;
		_currentArena = currentArena;
		ArenaMotor = Arenas[currentArena].Motor;
		ArenaMotor.Setup(this, Arenas[currentArena]);
		ArenaMotor.Initialize(this, Arenas[currentArena]);
	}

	public void PlayerSpawned() {
		_startGame = true;
	}
}
