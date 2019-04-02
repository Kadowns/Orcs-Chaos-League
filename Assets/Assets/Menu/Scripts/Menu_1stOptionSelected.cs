using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_1stOptionSelected : MonoBehaviour
{
    private EventSystem _eventSystem;

    public GameObject[] MenuOptions;

    public GameObject currentMenu, lastMenu, menuStaticObj, optStaticObj, mapStaticObj;

    public GameObject selectedButton, mapSelectedButton;

    private Player PiM;
    
    private Player[] PlayerArray = new Player[8];

    private bool[] playerActive = new bool[8];
    private bool[] botActive = new bool[8];

    public Sprite[] sprites = new Sprite[43];
    
    bool changer = true;
    bool changer1 = true;

    void Start()
    {      
        _eventSystem = EventSystem.current;

        currentMenu = GameObject.FindGameObjectWithTag("Cell");

        lastMenu = currentMenu;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        _eventSystem.SetSelectedGameObject(MenuOptions[0]);
   
        PiM = ReInput.players.GetPlayer(0);
        
        for (int x = 0; x < 8; x++)                                      
        {                                                                
            PlayerArray[x] = ReInput.players.GetPlayer(x);                
        }                                                                                                                                              
                                                                               
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
                    
                    for (int x = 0; x < 8; x++) {
                        botActive[x] = false;
                        playerActive[x] = false;
                    }     
                    break;

                case "EntradaPlayersDefinitivo":
                    lastMenu = mapStaticObj;       
                    MenuOptions = GameObject.FindGameObjectsWithTag("Test");
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

        if (PiM.GetButtonDown("UICancel") && currentMenu.name != "MainMenu" )
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

        if (currentMenu.name == "EntradaPlayersDefinitivo")
        {
            ChangePlayerSprite();
        }
    }

    public void ChangeMap()
    {
        MapData.SelectedMap = 0;
        
        if (currentMenu.name == "MapSelect")
        {
            mapSelectedButton = selectedButton;
        }

        switch (mapSelectedButton.name)
        {
            case "Map1":
                MapData.SelectedMap = 0;
            break;
            case "Map2":
                MapData.SelectedMap = 1;
            break;      
            case "Map3":
                MapData.SelectedMap = 2;
            break;      
            case "Map4":
                MapData.SelectedMap = 3;                  
            break;   
            case "Map5":
                MapData.SelectedMap = 4;
            break;
            case "Map6":
                MapData.SelectedMap = 5;
            break;             
        }                                
    }

    public void ChangePlayerSprite()
    {
        changer = true;
        
        for (int x = 0; x < 8; x++)
        {
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("UISubmit") && changer == true && playerActive[x] == false)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+5];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+9];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+13];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+17];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+21];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+25];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+29];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+33];
                        playerActive[x] = true;
                        botActive[x] = false;
                    break;  
                }
                changer = false;
            }
            
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("AddBot") && changer == true && playerActive[x] == false && botActive[x] == false)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+3];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+8];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+12];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+16];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+20];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+24];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+28];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+32];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+36];
                        playerActive[x] = false;
                        botActive[x] = true;
                        break;  
                }
                changer = false;
            }        
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("RemoveBot") && changer == true && botActive[x] == true)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+4];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+9];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+13];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+17];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+21];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+25];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+29];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+33];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+37];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                }
                changer = false;
            }
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("RemoveBot") && changer == true && playerActive[x] == true)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+4];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+9];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+13];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+17];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+21];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+25];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+29];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+33];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+37];
                        botActive[x] = false;
                        playerActive[x] = false;
                        break;  
                }
                changer = false;
            }
        }
    }
}