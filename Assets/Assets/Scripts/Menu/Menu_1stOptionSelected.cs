using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_1stOptionSelected : MonoBehaviour {


    private EventSystem _eventSystem;

    private Player player;

    private GameObject MainMenuGameObj, ModeSelectedObj;

    private Button startButtonEntradaPlayers;

    private string gameModeSelectedString, mapSelectedString;


    public GameObject GameModeSelected, mapSelected, menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    public Toggle menuToggle1, menuToggle2, menuToggle3;




    void Start () {

        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        MainMenuGameObj = GameObject.FindGameObjectWithTag("Cell");

        GameModeSelected = GameObject.FindGameObjectWithTag("Cell");

        mapSelected = GameObject.FindGameObjectWithTag("Cell");

        player = ReInput.players.GetPlayer(0);      

    }

    void Update() {

        if (!menuGameObj.activeSelf)
        {
            MenuOptions = GameObject.FindGameObjectsWithTag("Test");

            lastMenuGameObj = menuGameObj;

            menuGameObj = GameObject.FindGameObjectWithTag("Cell");

            if (menuGameObj.name == "ModeSelect")
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            else
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[0]);
            }
        }

        if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "ModeSelect")
        {
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
        }

        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(MenuOptions[0]); 
        }
        
        switch(menuGameObj.name)
        {

            //Mapa 1.1
            case "ModeSelect":

              //  DoAndCancel();

                ModeSelectedObj = GameObject.FindGameObjectWithTag("Cell");
              
                GameModeSelected = _eventSystem.currentSelectedGameObject;
                
                pressedbuttonReturn();
            break;

            //Mapa 1.2
            case "MapSelect":
                mapSelected = _eventSystem.currentSelectedGameObject;

                ReturnMapSelected();

                pressedbuttonReturn();
            break;

            //Mapa 1.3
            case "EntradaPlayers":
                ReturnGameMode();

                startButtonEntradaPlayers = FindObjectOfType<Button>();

                if (menuToggle1.isOn == false && menuToggle2.isOn == false && menuToggle3.isOn == false)
                {
                    startButtonEntradaPlayers.interactable = false;
                }

                pressedbuttonReturn();
            break;

            //Mapa 2
            case "Options":
                pressedbuttonReturn();
            break;

            //Mapa 2.1
            case "SoundOptions":
                pressedbuttonReturn();
            break;

            //Mapa 2.2
            case "GraphicsOptions":
                pressedbuttonReturn();
            break;

            //Mapa 3.1
            case "ReallyQuit":
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

        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "EntradaPlayers" && menuGameObj.name == "MapSelect")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = menuGameObj;
          // .SetActive(false);
            ModeSelectedObj.SetActive(true);
        }


        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "MapSelect" && menuGameObj.name == "ModeSelect")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = menuGameObj;
            // .SetActive(false);
            MainMenuGameObj.SetActive(true);
        }


        if (player.GetButton("UICancel") && lastMenuGameObj.name == "MapSelect" && menuGameObj.name == "EntradaPlayers")
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

    public string ReturnMapSelected()
    {
        mapSelectedString = mapSelected.name;
        return mapSelectedString;
    }

}
   
