using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_1stOptionSelected : MonoBehaviour {


    private EventSystem _eventSystem;

    private Player player;

    private GameObject MainMenuGameObj;

    private Button startButtonEntradaPlayers;

    private string gameModeSelectedString;


    public GameObject GameModeSelected;
    
    public GameObject menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    public Toggle menuToggle1, menuToggle2, menuToggle3;




    void Start () {

        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        MainMenuGameObj = GameObject.FindGameObjectWithTag("Cell");

        GameModeSelected = GameObject.FindGameObjectWithTag("Cell");

        player = ReInput.players.GetPlayer(0);      

    }

    void Update() {

        if (!menuGameObj.activeSelf)
        {
            MenuOptions = GameObject.FindGameObjectsWithTag("Test");

            lastMenuGameObj = menuGameObj;

            menuGameObj = GameObject.FindGameObjectWithTag("Cell");

            _eventSystem.SetSelectedGameObject(MenuOptions[0]);
        }     	

        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(MenuOptions[0]); 
        }
        
        switch(menuGameObj.name)
        {
            case "EntradaPlayers":
            //    ReturnGameMode();

                startButtonEntradaPlayers = FindObjectOfType<Button>();

                if (menuToggle1.isOn == false && menuToggle2.isOn == false && menuToggle3.isOn == false)
                {
                    startButtonEntradaPlayers.interactable = false;
                }

                pressedbuttonReturn();
            break;

            case "Options":
                pressedbuttonReturn();
            break;

            case "ModeSelect":
                GameModeSelected = _eventSystem.currentSelectedGameObject;
                pressedbuttonReturn();
            break;

            case "ReallyQuit":
                pressedbuttonReturn();
            break;

            case "SoundOptions":
                pressedbuttonReturn();
            break;

            case "GraphicsOptions":
                pressedbuttonReturn();
            break;
        }

    }    


    public void pressedbuttonReturn()
    {
        if(player.GetButtonDown("UICancel"))
        {
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if(player.GetButtonDown("UICancel") && lastMenuGameObj.name == "EntradaPlayers" && menuGameObj.name == "ModeSelect")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = MainMenuGameObj;
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "SoundOptions" && menuGameObj.name == "Options")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = MainMenuGameObj;
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "GraphicsOptions" && menuGameObj.name == "Options")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = MainMenuGameObj;
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if (player.GetButton("UICancel") && lastMenuGameObj.name == "ModeSelect" && menuGameObj.name == "EntradaPlayers")
        {
            menuToggle1.isOn = false;
            menuToggle2.isOn = false;
            menuToggle3.isOn = false;

            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }
    }

    public string ReturnGameMode()
    {
        gameModeSelectedString = GameModeSelected.name;
        return gameModeSelectedString;
    }
}
   
