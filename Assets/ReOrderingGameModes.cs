using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReOrderingGameModes : MonoBehaviour {

  //  public GameObject gameMode1, gameMode2, gameMode3, gameMode4, gameMode5;

    public GameObject GameModeSelectionScene;

    public GameObject[] listona;

    private Player dahPlayer;

    private bool state = false;

    private EventSystem _eventSystem;


    // Use this for initialization
    void Start () {

        dahPlayer = ReInput.players.GetPlayer(0);

        _eventSystem = EventSystem.current;

    }
	
	// Update is called once per frame
	void Update () {
        	
        if(GameModeSelectionScene.name == "ModeSelect" && state == false)
        {
            /*
            listona[0] = GameObject.Find("ModeBloco5");
            listona[1] = GameObject.Find("ModeBloco1");
            listona[2] = GameObject.Find("ModeBloco2");
            listona[3] = GameObject.Find("ModeBloco3");
            listona[4] = GameObject.Find("ModeBloco4");
            */

            listona = GameObject.FindGameObjectsWithTag("Test");
            

        }

        _eventSystem.SetSelectedGameObject(listona[1]);

        if (dahPlayer.GetButtonDown("UIHorizontal"))
        {

            for (int i = 0; i < listona.Length; i++)
            {
                if (listona[i] != listona[4])
                {
                    listona[i] = listona[i + 1];
                }
                else
                {
                    listona[4] = listona[0];
                }
            }
      
        }

        /*

        if (dahPlayer.GetNegativeButtonDown("UIHorizontal"))
        {

            for (int i = 5; i > listona.Length - 4; i--)
            {
                if (listona[i] != listona[0])
                {
                    listona[i] = listona[i - 1];
                }
                else
                {
                    listona[0] = listona[4];
                }
            }

        }
        */

    }
}
