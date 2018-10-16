using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour {

    private void Start()
    {
            SceneManager.LoadScene(1);
    }

    public void SetPlayers(int value) {
        for (int i = 0; i < PlayerSelection.PlayersInGame.Length; i++) {
            PlayerSelection.PlayersInGame[i] = i < value;
        }
    }

}
