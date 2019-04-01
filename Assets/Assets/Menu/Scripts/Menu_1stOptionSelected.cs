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

    public GameObject currentMenu, lastMenu, menuStaticObj, optStaticObj;

    public GameObject selectedButton, mapSelectedButton;

    private Player PiM;
    
    private Player[] PlayerArray = new Player[8];

    public Sprite[] sprites = new Sprite[43];
    
    bool changer = true;  

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

        if (currentMenu.name == "EntradaPlayersDefinitivo")
        {
            ChangePlayerSprite();
        }
    }

    public void ChangeMap()
    {
        for (int x = 0; x < 6; x++)
        {
            MapData.SelectedMap[x] = false;
        }
        
        if (currentMenu.name == "MapSelect")
        {
            mapSelectedButton = selectedButton;
        }

        switch (mapSelectedButton.name)
        {
            case "Map1":
                MapData.SelectedMap[0] = true;
            break;
            case "Map2":
                MapData.SelectedMap[1] = true;
            break;      
            case "Map3":
                MapData.SelectedMap[2] = true;
            break;      
            case "Map4":
                MapData.SelectedMap[3] = true;                  
            break;   
            case "Map5":
                MapData.SelectedMap[4] = true;
            break;
            case "Map6":
                MapData.SelectedMap[5] = true;
            break;             
        }
        
        /*
        Debug.Log(mapSelectedButton.name);
        
        for (int x = 0; x < 6; x++)
        {
            Debug.Log(MapData.SelectedMap[x]);
        } 
        */                                      
    }

    public void ChangePlayerSprite()
    {
        changer = true;
        
        for (int x = 0; x < 8; x++)
        {
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("UISubmit") && changer == true)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x];
                    break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+5];
                    break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+9];
                    break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+13];
                    break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+17];
                    break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+21];
                    break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+25];
                    break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+29];
                    break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+33];
                    break;  
                }
                changer = false;
            }
            if (MenuOptions[x].name == selectedButton.name && PiM.GetButtonDown("AddBot") && changer == true)
            {
                var xis = GameObject.Find(selectedButton.name);
                switch (x)
                {
                    case 0:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+3];
                        break;
                    case 1:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+8];
                        break;
                    case 2:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+12];
                        break;
                    case 3:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+16];
                        break;
                    case 4:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+20];
                        break;     
                    case 5:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+24];
                        break;  
                    case 6:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+28];
                        break;  
                    case 7:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+32];
                        break;  
                    case 8:
                        xis.gameObject.GetComponent<Image>().sprite = sprites[x+36];
                        break;  
                }
                changer = false;
            }
        }
    }
}