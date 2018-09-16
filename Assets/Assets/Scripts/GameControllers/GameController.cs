using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameController : Singleton<GameController> {

	public PlayerController[] PlayerControllers;
	public int CurrentArena = 0;
	
    [SerializeField] private GameObject _hub;
	[SerializeField] private GameObject _scenario1;
	[SerializeField] private float _hubWait = 3;
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
	
	private bool _rematch, _goToHub, _paused, _gameOverMenu, _transition;

    private bool boolNathan;
	
    private void Start() {
        
        
	    _camera = CameraController.Instance;
	    _fx = ScreenEffects.Instance;
	    _music = MusicController.Instance;
	    _hud = HUDController.Instance;
	    
	    SetGameState(66);
	    _music.PlayBgmByIndex(1);
	    _music.SetBGMLowPassFilter(200);
	    _fx.Blur(0.5f, new Color(0.9f, 0.9f, 0.9f));
	    _fx.SetCameraAnimationTrigger("ScanLines");
    }
	

	private void Update () {
		
		switch (_gameState) {
			case -1:
				_gameOverTimer += Time.deltaTime;
				if (_gameOverDelay < _gameOverTimer) {
					if (!_gameOverMenu) {
						_hud.EnableMenuByIndex(1);
						_hud.FighterHud(false);
						_gameOverMenu = true;
                        boolNathan = true;          
					}
						
					if (_goToHub || _rematch) {
						_gameOverTimer = 0;
						_gameOverMenu = false;
                        boolNathan = false;
						_fx.Blur(0f, Color.white);
					
						_hud.ResetToDefault(_goToHub);
			
						ChangePlayersInput("UI", "Default");
						var orcs = GameObject.FindGameObjectsWithTag("Player");
						foreach (var orc in orcs) {
							orc.SetActive(false);
						}
						_hud.EnableMenuByIndex(-1);
						if (_rematch) {
							for (int i = 0; i < PlayerControllers.Length; i++) {
								if (!PlayerControllers[i].PlayerInGame)
									continue;
							
								PlayerControllers[i].gameObject.SetActive(true);
								PlayerControllers[i].ResetToDefault(true);
							}
							SetGameState(1);
							_rematch = false;
						}
					
						if (_goToHub) {
							_camera.DoTransition("TransitionToHub");
							_activePlayers = 0;
							_readyPlayers = 0;
							for (int i = 0; i < PlayerControllers.Length; i++) {
								PlayerControllers[i].gameObject.SetActive(true);
								PlayerControllers[i].ResetToDefault(false);
							}
							SetGameState(0);
							_goToHub = false;
							_camera.MaxZoom(false);
						}
					}
				}
				
				break;
			case 0: // HUB
				
				if (_activePlayers > 1 && _activePlayers <= _readyPlayers) {
					MonitorProxy.Instance.Anima.SetBool("Countdown", true);
					MonitorProxy.Instance.DoScroll(false);
					_timeToStart += Time.deltaTime;

					if (_timeToStart > _hubWait) {
						SetGameState(1);
						_camera.DoTransition("TransitionToScenario");
						_fx.SetNotHubAnimationTrigger("Stop");
						_timeToStart = 0;
					}		
				}
				else {
					MonitorProxy.Instance.Anima.SetBool("Countdown", false);
					MonitorProxy.Instance.DoScroll(true);
				}

				break;
		}	
	}

	public void SetGameState(int state) {
		_gameState = state;
		_camera.UpdateGameState(state);
		switch (state) {
			case -1:
				
				
				GlobalAudio.Instance.StopLoop();
				_music.ChangeLowPassFilterFrequency(900f, 1f);
				_fx.Blur(20f, Color.gray);
				
				_camera.MaxZoom(true); 
				_fx.SetCameraAnimationTrigger("ScanLines");
				ChangePlayersInput("Default", "UI");
				break;

			case 0: 	
				_fx.SetCameraAnimationTrigger("StopFx");
				_fx.SetNotHubAnimationTrigger("Idle");
				_fx.Blur(0f, Color.white);			
				MonitorProxy.Instance.Anima.SetTrigger("Reset");
				_camera.SceneCenter(_hub.transform);
				_camera.DoTransition("Intro");
				_music.ChangeLowPassFilterFrequency(400f, 0.5f);
				
				break;
			case 1:
						
				_camera.SceneCenter(_scenario1.transform);
				
				
				_music.DramaticFrequencyChange(0.5f, 4.5f, 1f, 600f, 250f);
				GlobalAudio.Instance.LoopByIndex(2);

				for (int i = 0; i < PlayerControllers.Length; i++) {
					if (!PlayerControllers[i].PlayerInGame) {

						PlayerControllers[i].gameObject.SetActive(false);
						continue;
					}

					PlayerControllers[i].SetDefaultPosition(_scenario1.transform.position);
					PlayerControllers[i].StartSpawning();
				}
				
				ArenaController.Instance.PrepareGame(ref PlayerControllers, CurrentArena);
				break;
		}
		
		foreach (var controller in PlayerControllers) {
			
			controller.UpdateGameState(_gameState);
		}
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

	public void SetArena(int index) {
		CurrentArena = index;
	}

	public void StartGame() {
		SetGameState(0);
		_hud.LogoAnimation("Hide");
	}
	
	public void GoToHub() {
		_goToHub = true;
	}

	public void Rematch() {
		_rematch = true;
	}

	public void IncreaseReadyPlayers() {
		_timeToStart = 0;
		_readyPlayers++;
	}

	public void IncreaseActivePlayers() {
		
		_hud.UpdateFighterHudPosition();
		_activePlayers++;
	}
	
	public void DecreaseReadyPlayers() {
		_readyPlayers--;
	}

	public void DecreaseActivePlayers() {
		_hud.UpdateFighterHudPosition();
		_activePlayers--;
	}

	public int GetGameState() {
		return _gameState;
	}


}

