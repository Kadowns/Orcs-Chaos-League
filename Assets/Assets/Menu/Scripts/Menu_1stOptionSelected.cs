using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_1stOptionSelected : MonoBehaviour
{
    private EventSystem _eventSystem;

    public GameObject[] MenuOptions;

    public GameObject currentMenu, lastMenu, menuStaticObj, optStaticObj;

    public GameObject selectedButton;

    private Player PiM;

    void Start()
    {
        
        //MusicController.Instance.PlayBgmByIndex(0);
        _eventSystem = EventSystem.current;

        currentMenu = GameObject.FindGameObjectWithTag("Cell");

        lastMenu = currentMenu;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        _eventSystem.SetSelectedGameObject(MenuOptions[0]);
   
        PiM = ReInput.players.GetPlayer(0);

        UpdateOpcoesMenu();
    }

    public void UpdateOpcoesMenu()
    {
        MenuOptions = GameObject.FindGameObjectsWithTag("Test");
  
        currentMenu = GameObject.FindGameObjectWithTag("Cell");

            switch (currentMenu.name)
            {
                case "MainMenu":
                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
                    currentMenu = GameObject.FindGameObjectWithTag("Cell");

                    lastMenu = currentMenu;
                    break;

                case "MapSelect":
                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
                    currentMenu = GameObject.FindGameObjectWithTag("Cell");

                    lastMenu = menuStaticObj;
                    break;

                case "EntradaPlayersDefinitivo":
                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
                    lastMenu = menuStaticObj;

                    currentMenu = GameObject.FindGameObjectWithTag("Cell");
                    break;

                case "Options":
                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
                    lastMenu = menuStaticObj;

                    currentMenu = GameObject.FindGameObjectWithTag("Cell");
                    break;

                case "ReallyQuit":
                    lastMenu = menuStaticObj;

                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
                    currentMenu = GameObject.FindGameObjectWithTag("Cell");
                    break;
        }

        if (PiM.GetButtonDown("UICancel") && currentMenu.name != "MainMenu")
        {
            currentMenu.SetActive(false);
            lastMenu.SetActive(true);
            lastMenu = currentMenu;

            MenuOptions = GameObject.FindGameObjectsWithTag("Test");
            currentMenu = GameObject.FindGameObjectWithTag("Cell");
        }
    }

    void Update()
    {
        selectedButton = _eventSystem.currentSelectedGameObject;

        if (_eventSystem.currentSelectedGameObject == null || _eventSystem.currentSelectedGameObject.active == false)
        {
            _eventSystem.SetSelectedGameObject(MenuOptions[0]);
        }

        if (PiM.GetButtonDown("UICancel") && currentMenu.name != "MainMenu")
        {
            UpdateOpcoesMenu();
        }
    }
}