using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_1stOptionSelected : MonoBehaviour {

    private EventSystem _eventSystem;

  //  public Button Fbutton;

    [SerializeField] private MenuBehaviour[] _menu;

    // Use this for initialization
    void Start () {
        _eventSystem = EventSystem.current;
        
    }


    // Update is called once per frame
    void Update() {

       // Fbutton = Fbutton.GetComponent<Button>();
		
	}

    public void EnableMenuByIndex(int index)
    {
        _eventSystem.SetSelectedGameObject(null);
        if (index >= 0 && index < _menu.Length)
        {

            _menu[index].Enable(true);
            _eventSystem.firstSelectedGameObject = _menu[index].Buttons[0];
            _eventSystem.SetSelectedGameObject(_menu[index].Buttons[0], new BaseEventData(_eventSystem));
        }

    }
}
