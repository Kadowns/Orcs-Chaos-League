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

    private Player p1, p2, p3, p4;

    private Button startButtonEntradaPlayers;

    private string mapSelectedString;

    private GameObject obj5, obj1, obj2, obj3, obj4, ButtonSelector1, ButtonSelector2, ButtonSelector3;

    



    

    public Sprite mapS_rock, mapS_burning, mapS_chaos, mapS_king, mapS_runner;

    public Sprite p1_cpu, p2_cpu, p3_cpu, p4_cpu;

    public Sprite p1_off, p2_off, p3_off, p4_off;

    public Sprite p1_on, p2_on, p3_on, p4_on;

    public GameObject MainMenuGameObj;

    public GameObject mapSelected, menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    public Toggle menuToggle1, menuToggle2, menuToggle3;

    public Button startConfirm;

    public bool p1_active_bot = false;

    public bool p1_active_p = false;

    public bool p2_active_bot = false;

    public bool p2_active_p = false;

    public bool p3_active_bot = false;

    public bool p3_active_p = false;

    public bool p4_active_bot = false;

    public bool p4_active_p = false;

    int contador = 0;







    void Start()
    {

        //GettingSprites();

       


        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        mapSelected = GameObject.FindGameObjectWithTag("Cell");

        _eventSystem.SetSelectedGameObject(MenuOptions[0]);

        if (p1 == null || p2 == null || p3 == null || p4 == null)
        {
            p1 = ReInput.players.GetPlayer(0);
            p2 = ReInput.players.GetPlayer(1);
            p3 = ReInput.players.GetPlayer(2);
            p4 = ReInput.players.GetPlayer(3);
        }
    }

    void Update()
    {

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

                startConfirm.interactable = false;

                p1_active_bot = false;
                p1_active_p = false;

                p2_active_bot = false;
                p2_active_p = false;

                p3_active_bot = false;
                p3_active_p = false;

                p4_active_bot = false;
                p4_active_p = false;

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

            case "EntradaPlayersDefinitivo":
                playersConfirmation();

                if(p1.GetButtonDown("Submit")|| p1.GetButtonDown("AddBot"))
                {
                    confirming();
                }

                if (p2.GetButtonDown("Submit") || p2.GetButtonDown("AddBot"))
                {
                    confirming();
                }

                if (p3.GetButtonDown("Submit") || p3.GetButtonDown("AddBot"))
                {
                    confirming();
                }

                if (p4.GetButtonDown("Submit") || p4.GetButtonDown("AddBot"))
                {
                    confirming();
                }

                pressedbuttonReturn();


                for(int i = 0; i < 4; i++)
                {
                    bool ifistrue = true; ;

                    if(PlayerData.CPU[i] == true && ifistrue == true)
                    {
                        contador++;
                    }

                    if(PlayerData.PlayersInGame[i] == true && ifistrue == true)
                    {
                        contador++;
                        ifistrue = false;
                    }

                    if (contador >= 4 && ifistrue == true)
                    {
                        startConfirm.interactable = true;
                        if (p1.GetButtonDown("Start") || p2.GetButtonDown("Start") || p3.GetButtonDown("Start") || p4.GetButtonDown("Start"))
                        {
                            SceneManager.LoadScene(1);
                        }
                    }
                }
                

             

               
            
           

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

            _eventSystem.SetSelectedGameObject(MenuOptions[0]);
        }

    }


    public void pressedbuttonReturn()
    {

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "MainMenu" && menuGameObj.name == "EntradaPlayersDefinitivo")
        {
            ///   bool pranbugar = true;

            var w = GameObject.Find("player1");
            var x = GameObject.Find("player2");
            var y = GameObject.Find("player3");
            var z = GameObject.Find("player4");

            startConfirm.interactable = false;

            if (w.GetComponent<Image>().sprite != p1_off)
            {
                w.GetComponent<Image>().sprite = p1_off;
                x.GetComponent<Image>().sprite = p2_off;
                y.GetComponent<Image>().sprite = p3_off;
                z.GetComponent<Image>().sprite = p4_off;
            }
            else
            {
                w.GetComponent<Image>().sprite = p1_off;
                x.GetComponent<Image>().sprite = p2_off;
                y.GetComponent<Image>().sprite = p3_off;
                z.GetComponent<Image>().sprite = p4_off;

                p1_active_bot = true;
                p1_active_p = false;

                p2_active_bot = true;
                p2_active_p = false;

                p3_active_bot = true;
                p3_active_p = false;

                p4_active_bot = true;
                p4_active_p = false;



                lastMenuGameObj.SetActive(true);
                menuGameObj.SetActive(false);
            }
        }

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "MainMenu" && menuGameObj.name == "ReallyQuit")
        {
            MainMenuGameObj.SetActive(true);
            menuGameObj.SetActive(false);
        }

        if (p1.GetButtonDown("Start") || p1.GetButtonDown("UICancel") || p1.GetButtonDown("UISubmit") && menuGameObj.name == "PressStart")
        {
            menuGameObj.SetActive(false);
            MainMenuGameObj.SetActive(true);
        }

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "SoundOptions" && menuGameObj.name == "Options")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = MainMenuGameObj;
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "GraphicsOptions" && menuGameObj.name == "Options")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = MainMenuGameObj;
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "MainMenu" && menuGameObj.name == "MapSelect")
        {
            lastMenuGameObj.SetActive(true);
            menuGameObj.SetActive(false);

            if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
            {
                _eventSystem.SetSelectedGameObject(MenuOptions[0]);
            }

        }


        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "EntradaPlayers" && menuGameObj.name == "MapSelect")
        {
            lastMenuGameObj.SetActive(false);
            lastMenuGameObj = menuGameObj;
            MainMenuGameObj.SetActive(true);
            //   mapSelected.SetActive(true);
        }

        if (p1.GetButtonDown("UICancel") && lastMenuGameObj.name == "MapSelect" && menuGameObj.name == "EntradaPlayers")
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

        if (p1.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_runner && tomarnorabo == true)
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


        if (p1.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_rock && tomarnorabo == true)
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

        if (p1.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_burning && tomarnorabo == true)
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


        if (p1.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_king && tomarnorabo == true)
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

        if (p1.GetButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_chaos && tomarnorabo == true)
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



        if (p1.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_runner && tomarnorabo == true)
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

        if (p1.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_chaos && tomarnorabo == true)
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

        if (p1.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_king && tomarnorabo == true)
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


        if (p1.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_burning && tomarnorabo == true)
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

        if (p1.GetNegativeButtonDown("UIHorizontal") && obj1.GetComponent<Image>().sprite == mapS_rock && tomarnorabo == true)
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

    public void playersConfirmation()
    {

        if (p1.GetButtonDown("UISubmit"))
        {
            var a = GameObject.Find("player1");
            a.GetComponent<Image>().sprite = p1_on;
            confirming();
          
        }
        if (p2.GetButtonDown("UISubmit"))
        {
            var a = GameObject.Find("player2");
            a.GetComponent<Image>().sprite = p2_on;
            confirming();


        }
        if (p3.GetButtonDown("UISubmit"))
        {
            var a = GameObject.Find("player3");
            a.GetComponent<Image>().sprite = p3_on;
            confirming();

        }
        if (p4.GetButtonDown("UISubmit"))
        {
            var a = GameObject.Find("player4");
            a.GetComponent<Image>().sprite = p4_on;
            confirming();

        }

        if (p1.GetButtonDown("UICancel") && menuGameObj.name == "EntradaPlayersDefinitivo")
        {
            var a = GameObject.Find("player1");
            a.GetComponent<Image>().sprite = p1_off;
            confirming();
        }

        if (p2.GetButtonDown("UICancel") && menuGameObj.name == "EntradaPlayersDefinitivo")
        {
            var a = GameObject.Find("player2");
            a.GetComponent<Image>().sprite = p2_off;
            confirming();
        }
        

        if (p3.GetButtonDown("UICancel") && menuGameObj.name == "EntradaPlayersDefinitivo")
        {
            var a = GameObject.Find("player3");
            a.GetComponent<Image>().sprite = p3_off;
            confirming();
        }


        if (p4.GetButtonDown("UICancel") && menuGameObj.name == "EntradaPlayersDefinitivo")
        {
            var a = GameObject.Find("player4");
            a.GetComponent<Image>().sprite = p4_off;
            confirming();
        }

        var w = GameObject.Find("player1");
        var x = GameObject.Find("player2");
        var y = GameObject.Find("player3");
        var z = GameObject.Find("player4");

        ///////////////////////////////////////////
        /////////////////////////////////////////////ADD BOTS
        /////////////////////////////////////////////

        if (p1.GetButtonDown("AddBot"))
        {
            bool addbot = true;

            if (x.GetComponent<Image>().sprite == p2_off && addbot == true)
            {
                x.GetComponent<Image>().sprite = p2_cpu;
                confirming();
                addbot = false;
               
            }

            if (y.GetComponent<Image>().sprite == p3_off && addbot == true)
            {
                y.GetComponent<Image>().sprite = p3_cpu;
                confirming();
                addbot = false;
            
            }

            if (z.GetComponent<Image>().sprite == p4_off && addbot == true)
            {
                z.GetComponent<Image>().sprite = p4_cpu;
                confirming();
                addbot = false;
              
            }
        }

        if (p2.GetButtonDown("AddBot"))
        {
            bool addbot2 = true;

            if (w.GetComponent<Image>().sprite == p1_off && addbot2 == true)
            {
                w.GetComponent<Image>().sprite = p1_cpu;
                confirming();
                addbot2 = false;
    
            }

            if (y.GetComponent<Image>().sprite == p3_off && addbot2 == true)
            {
                y.GetComponent<Image>().sprite = p3_cpu;
                confirming();
                addbot2 = false;
      
            }

            if (z.GetComponent<Image>().sprite == p4_off && addbot2 == true)
            {
                z.GetComponent<Image>().sprite = p4_cpu;
                confirming();
                addbot2 = false;
          
            }
        }

        if (p3.GetButtonDown("AddBot"))
        {
            bool addbot3 = true;

            if (w.GetComponent<Image>().sprite == p1_off && addbot3 == true)
            {
                w.GetComponent<Image>().sprite = p1_cpu;
                confirming();
                addbot3 = false;
          
            }

            if (x.GetComponent<Image>().sprite == p2_off && addbot3 == true)
            {
                x.GetComponent<Image>().sprite = p2_cpu;
                confirming();
                addbot3 = false;
            
            }

            if (z.GetComponent<Image>().sprite == p4_off && addbot3 == true)
            {
                z.GetComponent<Image>().sprite = p4_cpu;
                confirming();
                addbot3 = false;
      
            }
        }

        if (p4.GetButtonDown("AddBot"))
        {
            bool addbot4 = true;

            if (w.GetComponent<Image>().sprite == p1_off && addbot4 == true)
            {
                w.GetComponent<Image>().sprite = p1_cpu;
                confirming();
                addbot4 = false;

            }

            if (x.GetComponent<Image>().sprite == p2_off && addbot4 == true)
            {
                x.GetComponent<Image>().sprite = p2_cpu;
                confirming();
                addbot4 = false;
         
            }

            if (y.GetComponent<Image>().sprite == p3_off && addbot4 == true)
            {
                y.GetComponent<Image>().sprite = p3_cpu;
                confirming();
                addbot4 = false;
    
            }
        }


        ///////////////////////////////////////////
        /////////////////////////////////////////////REMOVE BOTS
        /////////////////////////////////////////////


        bool removebot = true;

        if (p1.GetButtonDown("RemoveBot") && x.GetComponent<Image>().sprite == p2_cpu && removebot == true)
        {
            x.GetComponent<Image>().sprite = p2_off;
            p2_active_bot = false;
            confirming();
            removebot = false;
 
        }

        if (p1.GetButtonDown("RemoveBot") && y.GetComponent<Image>().sprite == p3_cpu && removebot == true)
        {
            y.GetComponent<Image>().sprite = p3_off;
            p3_active_bot = false;
            confirming();
            removebot = false;
    
        }

        if (p1.GetButtonDown("RemoveBot") && z.GetComponent<Image>().sprite == p4_cpu && removebot == true)
        {
            z.GetComponent<Image>().sprite = p4_off;
            p4_active_bot = false;
            confirming();
            removebot = false;

        }


        bool removebot2 = true;

        if (p2.GetButtonDown("RemoveBot") && w.GetComponent<Image>().sprite == p1_cpu && removebot2 == true)
        {
            w.GetComponent<Image>().sprite = p1_off;
            p1_active_bot = false;
            confirming();
            removebot2 = false;
     
        }

        if (p2.GetButtonDown("RemoveBot") && y.GetComponent<Image>().sprite == p3_cpu && removebot2 == true)
        {
            y.GetComponent<Image>().sprite = p3_off;
            p3_active_bot = false;
            confirming();
            removebot2 = false;
           
        }

        if (p2.GetButtonDown("RemoveBot") && z.GetComponent<Image>().sprite == p4_cpu && removebot2 == true)
        {
            z.GetComponent<Image>().sprite = p4_off;
            p4_active_bot = false;
            confirming();
            removebot2 = false;
        
        }



        bool removebot3 = true;

        if (p3.GetButtonDown("RemoveBot") && w.GetComponent<Image>().sprite == p1_cpu && removebot3 == true)
        {
            w.GetComponent<Image>().sprite = p1_off;
            p1_active_bot = false;
            confirming();
            removebot3 = false;
          
        }

        if (p3.GetButtonDown("RemoveBot") && x.GetComponent<Image>().sprite == p2_cpu && removebot3 == true)
        {
            x.GetComponent<Image>().sprite = p2_off;
            p2_active_bot = false;
            confirming();
            removebot3 = false;
         
        }

        if (p3.GetButtonDown("RemoveBot") && z.GetComponent<Image>().sprite == p4_cpu && removebot3 == true)
        {
            z.GetComponent<Image>().sprite = p4_off;
            p4_active_bot = false;
            confirming();
            removebot3 = false;
        
        }


        bool removebot4 = true;

        if (p3.GetButtonDown("RemoveBot") && w.GetComponent<Image>().sprite == p1_cpu && removebot4 == true)
        {
            w.GetComponent<Image>().sprite = p1_off;
            p1_active_bot = false;
            confirming();
            removebot4 = false;

        }

        if (p3.GetButtonDown("RemoveBot") && x.GetComponent<Image>().sprite == p2_cpu && removebot4 == true)
        {
            x.GetComponent<Image>().sprite = p2_off;
            p2_active_bot = false;
            confirming();
            removebot4 = false;
     
        }

        if (p3.GetButtonDown("RemoveBot") && y.GetComponent<Image>().sprite == p3_cpu && removebot4 == true)
        {
            y.GetComponent<Image>().sprite = p3_off;
            p3_active_bot = false;
            confirming();
            removebot4 = false;
  
        }





    }

    public void confirming()
    {
      //  bool desgraça = true;

        var w = GameObject.Find("player1");
        var x = GameObject.Find("player2");
        var y = GameObject.Find("player3");
        var z = GameObject.Find("player4");


       
        if(w.GetComponent<Image>().sprite == p1_on || w.GetComponent<Image>().sprite == p1_cpu)
        {
            if (w.GetComponent<Image>().sprite == p1_cpu)
            {
                p1_active_bot = true;
                p1_active_p = false;
                PlayerData.CPU[0] = true;
                PlayerData.PlayersInGame[0] = false;
            }
            
            if(w.GetComponent<Image>().sprite == p1_on)
            {
                p1_active_bot = false;
                p1_active_p = true;
                PlayerData.CPU[0] = false;
                PlayerData.PlayersInGame[0] = true;
            }
        }
        else
        {
            PlayerData.CPU[0] = false;
            PlayerData.PlayersInGame[0] = false;
        }


        if (x.GetComponent<Image>().sprite == p2_on || x.GetComponent<Image>().sprite == p2_cpu)
        {
            if (x.GetComponent<Image>().sprite == p2_cpu)
            {
                p2_active_bot = true;
                p2_active_p = false;
                PlayerData.CPU[1] = true;
                PlayerData.PlayersInGame[1] = false;
            }

            if (x.GetComponent<Image>().sprite == p2_on)
            {
                p2_active_bot = false;
                p2_active_p = true;
                PlayerData.CPU[1] = false;
                PlayerData.PlayersInGame[1] = true;
            }
        }
        else
        {
            PlayerData.CPU[1] = false;
            PlayerData.PlayersInGame[1] = false;
        }


        if (y.GetComponent<Image>().sprite == p3_on || y.GetComponent<Image>().sprite == p3_cpu)
        {
            if (y.GetComponent<Image>().sprite == p3_cpu)
            {
                p3_active_bot = true;
                p3_active_p = false;
                PlayerData.CPU[2] = true;
                PlayerData.PlayersInGame[2] = false;
            }
            
            if (y.GetComponent<Image>().sprite == p3_on)
            {
                p3_active_bot = false;  
                p3_active_p = true;
                PlayerData.CPU[2] = false;
                PlayerData.PlayersInGame[2] = true;
            } 
        }
        else
        {
            PlayerData.CPU[2] = false;
            PlayerData.PlayersInGame[2] = false;
        }


        if (z.GetComponent<Image>().sprite == p4_on || z.GetComponent<Image>().sprite == p4_cpu)
        {
            if (z.GetComponent<Image>().sprite == p4_cpu)
            {
                p4_active_bot = true;
                p4_active_p = false;
                PlayerData.CPU[3] = true;
                PlayerData.PlayersInGame[3] = false;
            }

            if (z.GetComponent<Image>().sprite == p4_on)
            {
                p4_active_bot = false;
                p4_active_p = true;
                PlayerData.CPU[3] = false;
                PlayerData.PlayersInGame[3] = true;
            }
        }
        else
        {
            PlayerData.CPU[3] = false;
            PlayerData.PlayersInGame[3] = false;
        }

     

     //   if (p1_active_p == true || p2_active_p == true || p3_active_p == true || p4_active_p == true || p1_active_p == true || p2_active_p == true || p3_active_p == true || p4_active_bot == true || p1_active_p == true || p2_active_p == true || p3_active_bot == true || p4_active_bot == true || p1_active_p == true || p2_active_p == true || p3_active_bot == true || p4_active_bot == true || p1_active_p == true || p2_active_bot == true || p3_active_bot == true || p4_active_bot == true)
     //   {
     //  startConfirm.interactable = true;
     //   }


    }

  


}







