using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_1stOptionSelected : MonoBehaviour {


    private EventSystem _eventSystem;

    private Player player;

    private Button startButtonEntradaPlayers;

    private string mapSelectedString;

    private GameObject obj5, obj1, obj2, obj3, obj4, ButtonSelector1, ButtonSelector2, ButtonSelector3;




    public GameObject MainMenuGameObj;

    public GameObject mapSelected, menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    public Toggle menuToggle1, menuToggle2, menuToggle3;

    public Sprite mapS_rock, mapS_burning, mapS_chaos, mapS_king, mapS_runner;




    void Start () {

        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        mapSelected = GameObject.FindGameObjectWithTag("Cell");

        player = ReInput.players.GetPlayer(0);

        _eventSystem.SetSelectedGameObject(MenuOptions[0]);

        

    }

    void Update() {
         
     //   if (_eventSystem.currentSelectedGameObject == null)
    //    {
     //       _eventSystem.SetSelectedGameObject(MenuOptions[0]); 
     //   }

    //    if (menuGameObj.name == "MainMenu")
   //     {
    //        _eventSystem.SetSelectedGameObject(MenuOptions[0]);
    //    }

        switch (menuGameObj.name)
        {
            case "PressStart":

                pressedbuttonReturn();

                if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }
                
                break;
                
            case "MainMenu":

                if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }

                break;

            //Mapa 1.2
            case "MapSelect":

                MapCycle();

                ReturnMapSelected();

                pressedbuttonReturn();
            break;
                
            //Mapa 1.3
            case "EntradaPlayers":
                //  ReturnGameMode();


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

                if (_eventSystem.currentSelectedGameObject == null)
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }

                pressedbuttonReturn();

            break;

        }

        if (!menuGameObj.activeSelf)
        {
            MenuOptions = GameObject.FindGameObjectsWithTag("Test");

            lastMenuGameObj = menuGameObj;

            menuGameObj = GameObject.FindGameObjectWithTag("Cell");

            if (menuGameObj.name == "MapSelect")
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            else
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[0]);
            }

        }

    }    


    public void pressedbuttonReturn()
    {
   
        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "MainMenu" && menuGameObj.name == "ReallyQuit")
        {
            MainMenuGameObj.SetActive(true);
            menuGameObj.SetActive(false);
        }

        if (player.GetButtonDown("UICancel") || player.GetButtonDown("UISubmit") && menuGameObj.name == "PressStart")
        {
            menuGameObj.SetActive(false);
            MainMenuGameObj.SetActive(true);
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

        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "MainMenu" && menuGameObj.name == "MapSelect")
        {
            lastMenuGameObj.SetActive(true);   
            menuGameObj.SetActive(false);

            if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[0]);
            }

        }


        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "EntradaPlayers" && menuGameObj.name == "MapSelect")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = menuGameObj;
            MainMenuGameObj.SetActive(true);
         //   mapSelected.SetActive(true);
        }

        if (player.GetButtonDown("UICancel") && lastMenuGameObj.name == "MapSelect" && menuGameObj.name == "EntradaPlayers")
        {
            menuToggle1.isOn = false;
            menuToggle2.isOn = false;
            menuToggle3.isOn = false;

            MainMenuGameObj.SetActive(false);
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }
    }
    
    public string ReturnMapSelected()
    {
       // mapSelectedString = mapSelected.name;
        return mapSelectedString;
    }

    public void MapCycle()
    {
        bool tomarnorabo = true;

        obj5 = GameObject.Find("LastMap");

        obj1 = GameObject.Find("MapActual");

        obj2 = GameObject.Find("SecondMap");

        obj3 = GameObject.Find("ThirdMap");

        obj4 = GameObject.Find("FourthMap");

        ///FORWARDS///////////////////////////////////////////////////////////////////////////////////////////////////

        if (player.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_runner && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_runner;
                obj1.GetComponent<Image>().sprite = mapS_rock;
                obj2.GetComponent<Image>().sprite = mapS_burning;
                obj3.GetComponent<Image>().sprite = mapS_king;
                obj4.GetComponent<Image>().sprite = mapS_chaos;
                tomarnorabo = false;
                mapSelectedString = "mapS_rock";
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
        

        if (player.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_rock && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_rock;
                obj1.GetComponent<Image>().sprite = mapS_burning;
                obj2.GetComponent<Image>().sprite = mapS_king;
                obj3.GetComponent<Image>().sprite = mapS_chaos;
                obj4.GetComponent<Image>().sprite = mapS_runner;
                tomarnorabo = false;
            mapSelectedString = "mapS_burning";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            
        if (player.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_burning && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_burning;
                obj1.GetComponent<Image>().sprite = mapS_king;
                obj2.GetComponent<Image>().sprite = mapS_chaos;
                obj3.GetComponent<Image>().sprite = mapS_runner;
                obj4.GetComponent<Image>().sprite = mapS_rock;
                tomarnorabo = false;
            mapSelectedString = "mapS_king";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }


        if (player.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_king && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_king;
                obj1.GetComponent<Image>().sprite = mapS_chaos;
                obj2.GetComponent<Image>().sprite = mapS_runner;
                obj3.GetComponent<Image>().sprite = mapS_rock;
                obj4.GetComponent<Image>().sprite = mapS_burning;
                tomarnorabo = false;
            mapSelectedString = "mapS_chaos";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }

        if (player.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_chaos && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_chaos;
                obj1.GetComponent<Image>().sprite = mapS_runner;
                obj2.GetComponent<Image>().sprite = mapS_rock;
                obj3.GetComponent<Image>().sprite = mapS_burning;
                obj4.GetComponent<Image>().sprite = mapS_king;
                tomarnorabo = false;
            mapSelectedString = "mapS_runner";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            
       

        ///BACKWARDS///////////////////////////////////////////////////////////////////////////////////////////////////

        
     
            if (player.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_runner && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_king;
                obj1.GetComponent<Image>().sprite = mapS_chaos;
                obj2.GetComponent<Image>().sprite = mapS_runner;
                obj3.GetComponent<Image>().sprite = mapS_rock;
                obj4.GetComponent<Image>().sprite = mapS_burning;
                tomarnorabo = false;
            mapSelectedString = "mapS_chaos";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }

            if (player.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_chaos && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_burning;
                obj1.GetComponent<Image>().sprite = mapS_king;
                obj2.GetComponent<Image>().sprite = mapS_chaos;
                obj3.GetComponent<Image>().sprite = mapS_runner;
                obj4.GetComponent<Image>().sprite = mapS_rock;
                tomarnorabo = false;
            mapSelectedString = "mapS_king";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }

            if (player.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_king && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_rock;
                obj1.GetComponent<Image>().sprite = mapS_burning;
                obj2.GetComponent<Image>().sprite = mapS_king;
                obj3.GetComponent<Image>().sprite = mapS_chaos;
                obj4.GetComponent<Image>().sprite = mapS_runner;
                tomarnorabo = false;
            mapSelectedString = "mapS_burning";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }


            if (player.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_burning && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_runner;
                obj1.GetComponent<Image>().sprite = mapS_rock;
                obj2.GetComponent<Image>().sprite = mapS_burning;
                obj3.GetComponent<Image>().sprite = mapS_king;
                obj4.GetComponent<Image>().sprite = mapS_chaos;
                tomarnorabo = false;
            mapSelectedString = "mapS_rock";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            
            if (player.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_rock && tomarnorabo == true)
            {
                obj5.GetComponent<Image>().sprite = mapS_chaos;
                obj1.GetComponent<Image>().sprite = mapS_runner;
                obj2.GetComponent<Image>().sprite = mapS_rock;
                obj3.GetComponent<Image>().sprite = mapS_burning;
                obj4.GetComponent<Image>().sprite = mapS_king;
                tomarnorabo = false;
            mapSelectedString = "mapS_runner";
            _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
    }

    public void ButtonVerify()
    {

        startButtonEntradaPlayers = FindObjectOfType<Button>();

        if (menuToggle1.isOn == true || menuToggle2.isOn == true && menuToggle3.isOn == true && startButtonEntradaPlayers.interactable == false)
        {
            startButtonEntradaPlayers.interactable = true;
        }

        if (menuToggle1.isOn == false && menuToggle2.isOn == false && menuToggle3.isOn == false)
        {
            startButtonEntradaPlayers.interactable = false;
        }

        if (menuToggle1.isOn == true)
        {
            ButtonSelector1 = GameObject.Find("SelectedPlayers");
        }
        if (menuToggle2.isOn == true)
        {
            ButtonSelector2 = GameObject.Find("SelectedPlayers2");
        }
        if (menuToggle3.isOn == true)
        {
            ButtonSelector3 = GameObject.Find("SelectedPlayers3");
        }

        if (ButtonSelector1.activeSelf == true && menuToggle1.isOn == false)
        {
            menuToggle1.isOn = true;
            startButtonEntradaPlayers.interactable = true;
        }


        if (ButtonSelector2.activeSelf == true && menuToggle2.isOn == false)
        {
            menuToggle2.isOn = true;
            startButtonEntradaPlayers.interactable = true;
        }


        if (ButtonSelector3.activeSelf == true && menuToggle3.isOn == false)
        {
            menuToggle3.isOn = true;
            startButtonEntradaPlayers.interactable = true;
        }
    }
}
   
