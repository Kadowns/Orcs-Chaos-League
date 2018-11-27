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

    private Player[] players = new Player[4];

    private Button startButtonEntradaPlayers;

    private GameObject[] mapSelectedReturn = new GameObject[1];

    private GameObject[] objs = new GameObject[5];

    private GameObject spriteGameObject, CurrentMapSelect;

    private bool dontChangeMenu = false, gambiarra = false;


    public GameObject MainMenuGameObj;

    public GameObject mapSelected, menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    public GameObject startConfirm;

    public bool[] players_active = new bool[4];

    public bool[] bot_active = new bool[4];



    void Start()
    {
        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        mapSelected = GameObject.FindGameObjectWithTag("Cell");

        _eventSystem.SetSelectedGameObject(MenuOptions[0]);

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    void Update()
    {
        if (!menuGameObj.activeSelf)
        {
            MenuOptions = GameObject.FindGameObjectsWithTag("Test");
            lastMenuGameObj = menuGameObj;
            menuGameObj = GameObject.FindGameObjectWithTag("Cell");
            if (menuGameObj.name == "EntradaPlayersDefinitivo" && startConfirm.activeSelf == true)
            {
                players_active[0] = false;
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
            else
            {
                players_active[0] = false;
                _eventSystem.SetSelectedGameObject(MenuOptions[0]);
            }
        }
        switch (menuGameObj.name)
        {
            case "PressStart":
                pressedbuttonReturn(menuGameObj, MainMenuGameObj);

                if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }
                break;

            case "MainMenu":
                gambiarra = true;
                for (int i = 0; i < 4; i++)
                {
                    bot_active[i] = false;
                    players_active[i] = false;
                }

                if (_eventSystem.currentSelectedGameObject == null && menuGameObj.name == "MainMenu")
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }
                break;

            case "MapSelect":
                MapCycle();

                ReturnMapSelected();

                pressedbuttonReturn(menuGameObj, MainMenuGameObj);
                break;

            case "EntradaPlayersDefinitivo":
                playersConfirmation();

                pressedbuttonReturn(menuGameObj, MainMenuGameObj);
                ChangingPlayersIngame();
                dontChangeMenu = false;
                break;

            case "ReallyQuit":
                if (_eventSystem.currentSelectedGameObject == null)
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }

                pressedbuttonReturn(menuGameObj, MainMenuGameObj);
                break;
        }
        if (!menuGameObj.activeSelf)
        {
            MenuOptions = GameObject.FindGameObjectsWithTag("Test");

            lastMenuGameObj = menuGameObj;

            menuGameObj = GameObject.FindGameObjectWithTag("Cell");

            players_active[0] = false;

            _eventSystem.SetSelectedGameObject(MenuOptions[0]);
        }
    }

    public void pressedbuttonReturn(GameObject toDeactivateMenu, GameObject toGoToMenu)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[0].GetButtonDown("UICancel") && dontChangeMenu == false)
            {
                Debug.Log("MudeiMesmo!!!!!!!!!!!!!!!");
                toDeactivateMenu.SetActive(false);
                toGoToMenu.SetActive(true);
                players_active[0] = false;

                if (menuGameObj.name == "EntradaPlayersDefinitivo" && startConfirm.activeSelf == true)
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[1]);
                }
                else
                {
                    _eventSystem.SetSelectedGameObject(MenuOptions[0]);
                }
            }
            ////POR ENQUANTO DEIXA ASSIM
            if (menuGameObj.name == "EntradaPlayersDefinitivo" && players_active[i] == true && gambiarra == true)
            {
                var a = GameObject.Find("1");
                a.GetComponent<Image>().sprite = fonteSpritesCode.p1_off;
                var b = GameObject.Find("2");
                b.GetComponent<Image>().sprite = fonteSpritesCode.p2_off;
                var c = GameObject.Find("3");
                c.GetComponent<Image>().sprite = fonteSpritesCode.p3_off;
                var d = GameObject.Find("4");
                d.GetComponent<Image>().sprite = fonteSpritesCode.p4_off;
                startConfirm.SetActive(false);
                players_active[0] = false;
                gambiarra = false;
            }
            //////////GAMBOSA ACIMA
        }
    }

    public GameObject ReturnMapSelected()
    {
        return mapSelectedReturn[0];
    }

    public void MapCycle()
    {
        bool tomarnorabo = true;

        objs[0].GetComponent<Image>().sprite = fonteSpritesCode.mapS_burning;
        objs[1].GetComponent<Image>().sprite = fonteSpritesCode.mapS_king;
        objs[2].GetComponent<Image>().sprite = fonteSpritesCode.mapS_chaos;
        objs[3].GetComponent<Image>().sprite = fonteSpritesCode.mapS_runner;
        objs[4].GetComponent<Image>().sprite = fonteSpritesCode.mapS_rock;

        for (int i = 0; i < players.Length; i++)
        {///FORWARDS///////////////////////////////////////////////////////////////////////////////////////////////////
            if (players[i].GetButtonDown("UIHorizontal") && tomarnorabo == true)
            {
                var lastIconSprite = objs[0].GetComponent<Image>().sprite;

                objs[0].GetComponent<Image>().sprite = objs[1].GetComponent<Image>().sprite;
                objs[1].GetComponent<Image>().sprite = objs[2].GetComponent<Image>().sprite;
                objs[2].GetComponent<Image>().sprite = objs[3].GetComponent<Image>().sprite;
                objs[3].GetComponent<Image>().sprite = objs[4].GetComponent<Image>().sprite;
                objs[4].GetComponent<Image>().sprite = lastIconSprite;

                CurrentMapSelect = GameObject.Find("ActualMap");
                tomarnorabo = false;
                mapSelectedReturn[0] = CurrentMapSelect;
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }///BACKWARDS///////////////////////////////////////////////////////////////////////////////////////////////////

            if (players[i].GetNegativeButtonDown("UIHorizontal") && tomarnorabo == true)
            {
                var lastIconSprite = objs[4].GetComponent<Image>().sprite;

                objs[4].GetComponent<Image>().sprite = objs[0].GetComponent<Image>().sprite;
                objs[0].GetComponent<Image>().sprite = objs[1].GetComponent<Image>().sprite;
                objs[1].GetComponent<Image>().sprite = objs[2].GetComponent<Image>().sprite;
                objs[2].GetComponent<Image>().sprite = objs[3].GetComponent<Image>().sprite;
                objs[3].GetComponent<Image>().sprite = lastIconSprite;

                CurrentMapSelect = GameObject.Find("ActualMap");
                tomarnorabo = false;
                mapSelectedReturn[0] = CurrentMapSelect;
                _eventSystem.SetSelectedGameObject(MenuOptions[1]);
            }
        }
    }

    public void playersConfirmation()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetButtonDown("UISubmit") && players_active[i] == false)
            {
                switch (i)
                {
                    case 0:
                        var a = GameObject.Find("1");
                        a.GetComponent<Image>().sprite = fonteSpritesCode.p1_on;
                        players_active[0] = true;
                        break;
                    case 1:
                        var b = GameObject.Find("2");
                        b.GetComponent<Image>().sprite = fonteSpritesCode.p2_on;
                        players_active[1] = true;
                        break;
                    case 2:
                        var c = GameObject.Find("3");
                        c.GetComponent<Image>().sprite = fonteSpritesCode.p3_on;
                        players_active[2] = true;
                        break;
                    case 3:
                        var d = GameObject.Find("4");
                        d.GetComponent<Image>().sprite = fonteSpritesCode.p4_on;
                        players_active[3] = true;
                        break;
                }
            }
            if (players[i].GetButtonDown("UICancel") && players_active[i] == true)
            {
                switch (i)
                {
                    case 0:
                        var b = GameObject.Find("1");
                        b.GetComponent<Image>().sprite = fonteSpritesCode.p1_off;
                        players_active[0] = false;
                        break;
                    case 1:
                        b = GameObject.Find("2");
                        b.GetComponent<Image>().sprite = fonteSpritesCode.p2_off;
                        players_active[1] = false;
                        break;
                    case 2:
                        b = GameObject.Find("3");
                        b.GetComponent<Image>().sprite = fonteSpritesCode.p3_off;
                        players_active[2] = false;
                        break;
                    case 3:
                        b = GameObject.Find("4");
                        b.GetComponent<Image>().sprite = fonteSpritesCode.p4_off;
                        players_active[3] = false;
                        break;
                }
                dontChangeMenu = true;
            }
            /////////////////////////////////////////////ADD BOTS
            if (players[i].GetButtonDown("AddBot") && players_active[i] == true)
            {
                bool botControl = true;
                switch (i)
                {
                    case 0:
                        var a = GameObject.Find("2");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p2_cpu;
                            botControl = false;
                            bot_active[1] = true;
                            players_active[1] = true;
                        }
                        a = GameObject.Find("3");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p3_cpu;
                            botControl = false;
                            bot_active[2] = true;
                            players_active[2] = true;
                        }
                        a = GameObject.Find("4");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p4_cpu;
                            botControl = false;
                            bot_active[3] = true;
                            players_active[3] = true;
                        }
                        break;
                    case 1:
                        a = GameObject.Find("1");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p1_cpu;
                            botControl = false;
                            bot_active[0] = true;
                            players_active[0] = true;
                        }
                        a = GameObject.Find("3");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p3_cpu;
                            botControl = false;
                            bot_active[2] = true;
                            players_active[2] = true;
                        }
                        a = GameObject.Find("4");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p4_cpu;
                            botControl = false;
                            bot_active[3] = true;
                            players_active[3] = true;
                        }
                        break;
                    case 2:
                        a = GameObject.Find("1");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p1_cpu;
                            botControl = false;
                            bot_active[0] = true;
                            players_active[0] = true;
                        }
                        a = GameObject.Find("2");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p2_cpu;
                            botControl = false;
                            bot_active[1] = true;
                            players_active[1] = true;
                        }
                        a = GameObject.Find("4");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p4_cpu;
                            botControl = false;
                            bot_active[3] = true;
                            players_active[3] = true;
                        }
                        break;
                    case 3:
                        a = GameObject.Find("1");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p1_cpu;
                            botControl = false;
                            bot_active[0] = true;
                            players_active[0] = true;
                        }
                        a = GameObject.Find("2");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p2_cpu;
                            botControl = false;
                            bot_active[1] = true;
                            players_active[1] = true;
                        }
                        a = GameObject.Find("3");
                        if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_off && botControl == true)
                        {
                            a.GetComponent<Image>().sprite = fonteSpritesCode.p3_cpu;
                            botControl = false;
                            bot_active[2] = true;
                            players_active[2] = true;
                        }
                        break;
                }
            }
            if (players[i].GetButtonDown("RemoveBot"))
            {
                bool botControl2 = true;

                if (botControl2 == true)
                {
                    switch (i)
                    {
                        case 0:
                            var a = GameObject.Find("2");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p2_off;
                                botControl2 = false;
                                bot_active[1] = false;
                                players_active[1] = false;
                            }
                            a = GameObject.Find("3");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p3_off;
                                botControl2 = false;
                                bot_active[2] = false;
                                players_active[2] = false;
                            }
                            a = GameObject.Find("4");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p4_off;
                                botControl2 = false;
                                bot_active[3] = false;
                                players_active[3] = false;
                            }
                            break;
                        case 1:
                            a = GameObject.Find("1");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p1_off;
                                botControl2 = false;
                                bot_active[0] = false;
                                players_active[0] = false;
                            }
                            a = GameObject.Find("3");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p3_off;
                                botControl2 = false;
                                bot_active[2] = false;
                                players_active[2] = false;
                            }
                            a = GameObject.Find("4");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p4_off;
                                botControl2 = false;
                                bot_active[3] = false;
                                players_active[3] = false;
                            }
                            break;
                        case 2:
                            a = GameObject.Find("1");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p1_off;
                                botControl2 = false;
                                bot_active[0] = false;
                                players_active[0] = false;
                            }
                            a = GameObject.Find("2");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p2_off;
                                botControl2 = false;
                                bot_active[1] = false;
                                players_active[1] = false;
                            }
                            a = GameObject.Find("4");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p4_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p4_off;
                                botControl2 = false;
                                bot_active[3] = false;
                                players_active[3] = false;
                            }
                            break;
                        case 3:
                            a = GameObject.Find("1");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p1_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p1_off;
                                botControl2 = false;
                                bot_active[0] = false;
                                players_active[0] = false;
                            }
                            a = GameObject.Find("2");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p2_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p2_off;
                                botControl2 = false;
                                bot_active[1] = false;
                                players_active[1] = false;
                            }
                            a = GameObject.Find("3");
                            if (a.GetComponent<Image>().sprite == fonteSpritesCode.p3_cpu && botControl2 == true)
                            {
                                a.GetComponent<Image>().sprite = fonteSpritesCode.p3_off;
                                botControl2 = false;
                                bot_active[2] = false;
                                players_active[2] = false;
                            }
                            break;
                    }
                }
            }
        }
    }

    public void ChangingPlayersIngame()
    {
        var y = 0;
     
        for (int e = 0; e < 4; e++)
        {

            if (players_active[e] == true)
            {
                if(bot_active[e] == true){
                    PlayerData.PlayersInGame[e] = players_active[e];
                    PlayerData.CPU[e] = bot_active[e];
                }
                else{
                    PlayerData.PlayersInGame[e] = players_active[e];
                }

                y++;
                if (y >= 2)
                {
                    startConfirm.SetActive(true);
                    if (players[e].GetButtonDown("Start"))
                    {
                        SceneManager.LoadScene(1);
                    }
                }
                else
                {
                    startConfirm.SetActive(false);
                }
            }
            else
            {
                if (bot_active[e] == false)
                {
                    PlayerData.PlayersInGame[e] = players_active[e];
                    PlayerData.CPU[e] = bot_active[e];
                }
                else
                {
                    PlayerData.PlayersInGame[e] = players_active[e];
                }
            }
            
         

        }
        
        Debug.Log(y);
    }
}