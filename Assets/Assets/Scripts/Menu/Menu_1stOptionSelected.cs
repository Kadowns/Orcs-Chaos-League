using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_1stOptionSelected : MonoBehaviour {


    private EventSystem _eventSystem;

    private Player p;

    public GameObject menuGameObj, lastMenuGameObj;

    public GameObject[] MenuOptions;

    void Start () {

        _eventSystem = EventSystem.current;

        MenuOptions = GameObject.FindGameObjectsWithTag("Test");

        menuGameObj = GameObject.FindGameObjectWithTag("Cell");

        p = ReInput.players.GetPlayer(0);


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
                pressedbuttonReturn();
            break;

            case "Options":
                pressedbuttonReturn();
            break;
        }
	}    


    public void pressedbuttonReturn()
    {
        if(p.GetButtonDown("UICancel"))
        {
            menuGameObj.SetActive(false);
            lastMenuGameObj.SetActive(true);
        }
        
    }

}
