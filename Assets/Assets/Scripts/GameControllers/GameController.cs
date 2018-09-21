using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameController : Singleton<GameController> {

	public PlayerController[] PlayerControllers;
	public bool[] ActivePlayers;
	public int CurrentArena = 0;
	
	[SerializeField] private GameObject _scenario1;
	[SerializeField] private float _gameOverDelay = 3;


    private FighterHUD _fhud;
    private PlayerController _pinput;
    private CameraController _camera;
	private ScreenEffects _fx;
	private MusicController _music;
	private HUDController _hud;

	private int _gameState = 66;
	private int _activePlayers = 0;
	private int _readyPlayers = 0;

    private int _playerInGame = 0;

    private float _timeToStart = 0f;
	private float _gameOverTimer = 0f;
	private float _suddenDeathTimer = 0f;
	
	private bool _rematch, _goToHub, _paused, _gameOverMenu, _transition, _matchEnded;
	
    private void Start() {
        
        
	    _camera = CameraController.Instance;
	    _fx = ScreenEffects.Instance;
	    _music = MusicController.Instance;
	    _hud = HUDController.Instance;
	    
	    _music.PlayBgmByIndex(1);
	    _music.SetBGMLowPassFilter(200);
	    _fx.Blur(0.5f, new Color(0.9f, 0.9f, 0.9f));
	    _fx.SetCameraAnimationTrigger("ScanLines");
	    StartMatch(ActivePlayers);
    }


	private void Update() {

		if (_matchEnded) {
			_gameOverTimer += Time.deltaTime;
			if (_gameOverDelay < _gameOverTimer) {
				if (!_gameOverMenu) {
					_hud.EnableMenuByIndex(1);
					_hud.FighterHud(false);
					_gameOverMenu = true;
				}

				if (_goToHub || _rematch) {
					_gameOverTimer = 0;
					_gameOverMenu = false;
					_fx.Blur(0f, Color.white);

					_hud.ResetToDefault();

					ChangePlayersInput("UI", "Default");
					var orcs = GameObject.FindGameObjectsWithTag("Player");
					foreach (var orc in orcs) {
						orc.SetActive(false);
					}

					_hud.EnableMenuByIndex(-1);
					if (_rematch) {
						StartMatch(ActivePlayers);
						_rematch = false;
					}

					if (_goToHub) {
						Debug.Log("VOLTEI PRO MENU");
					}
				}
			}
		}	
	}

	//Função chamada no inicio do jogo pra começar o jogo NÉ
	public void StartMatch(bool[] playersInGame) {
		_camera.SceneCenter(_scenario1.transform);			
		_music.DramaticFrequencyChange(0.5f, 4.5f, 1f, 600f, 250f);

		for (int i = 0; i < PlayerControllers.Length; i++) {
			if (!playersInGame[i]) {
				PlayerControllers[i].gameObject.SetActive(false);
				continue;
			}

			PlayerControllers[i].Hud.gameObject.SetActive(true);
			PlayerControllers[i].SetDefaultPosition(_scenario1.transform.position);
			PlayerControllers[i].StartSpawning();
		}
		_hud.UpdateFighterHudPosition();
				
		ArenaController.Instance.PrepareGame(ref PlayerControllers, CurrentArena);
		_matchEnded = false;
	}

	public void EndMatch() {


		_music.ChangeLowPassFilterFrequency(900f, 1f);
		_fx.Blur(20f, Color.gray);

		_camera.MaxZoom(true);
		_fx.SetCameraAnimationTrigger("ScanLines");
		ChangePlayersInput("Default", "UI");

		foreach (var controller in PlayerControllers) {

			controller.UpdateGameState(_gameState);
		}

		_matchEnded = true;
	}

	public void Pause(int playerNumber) {
		_paused = !_paused;
		if (_paused) {
			_hud.EnableMenuByIndex(0);
			_hud.PauseAnimation(playerNumber);
			ChangePlayersInput("Default", "UI");
			Time.timeScale = 0;
		}
		else {
			_hud.EnableMenuByIndex(-1);
			ChangePlayersInput("UI", "Default");
			Time.timeScale = 1;
		}
	}

	public void ChangePlayersInput(string inputToDisable, string inputToEnable) {
		
		foreach (var input in PlayerControllers) {
			input.ChangeInputMode(inputToDisable, inputToEnable);
		}
	}

	public void SetPlayersInput(string categoryName, bool value) {
		foreach (var input in PlayerControllers) {
			input.SetInputMode(categoryName, value);
		}
	}
	
	public void GoToHub() {
		_goToHub = true;
	}

	public void Rematch() {
		_rematch = true;
	}
}

