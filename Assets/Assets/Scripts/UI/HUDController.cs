using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDController : Singleton<HUDController> {


    private EventSystem _eventSystem;                   

    [SerializeField] private Transform _fightersHud;
    [SerializeField] private MenuBehaviour[] _menu;
    [SerializeField] private Animator _logo, _pausemenu, _pauseMenuPlayerNumber, _winnerSymbol, _blackout;

    private void Start() {
        _eventSystem = EventSystem.current;
        CameraController.Instance.OnIntroStarted += () => { BlackoutAnimation("FadeOut"); };
    }

    public void EnableMenuByIndex(int index) {
        _eventSystem.SetSelectedGameObject(null);
        if (index >= 0 && index < _menu.Length) {

            _menu[index].Enable(true);
            _eventSystem.firstSelectedGameObject = _menu[index].Buttons[0];
            _eventSystem.SetSelectedGameObject(_menu[index].Buttons[0], new BaseEventData(_eventSystem));
        }

        for (int i = 0; i < _menu.Length; i++) {
            if (i == index)
                continue;
            _menu[i].Enable(false);
        }
    }

    public void UpdateFighterHudPosition() {
        _fightersHud.GetComponent<LayoutReposition>().RecalculatePosition();
    }

    public void FighterHud(bool show) {
        if (show)
            _fightersHud.GetComponent<Animator>().SetTrigger("Show");
        else
            _fightersHud.GetComponent<Animator>().SetTrigger("Hide");
    }

    public void ResetToDefault() {
        _fightersHud.GetComponent<LayoutReposition>().ResetActiveChild();
    }

    public void PauseAnimation(int playerNumber) {
        _pauseMenuPlayerNumber.SetInteger("PlayerNumber", playerNumber);
    }

    public void BlackoutAnimation(string animation) {
        _blackout.SetTrigger(animation);
    }

    public void LogoAnimation(string animation) {
        _logo.SetTrigger(animation);
    }

    public void MenuAnimation(bool ispaused) {
        _pausemenu.SetBool("Bool", ispaused);
    }

    public void WinnerSymbol(int value) {
        Debug.Log(value);
        _winnerSymbol.SetInteger("StateChange", value);
    }
}
