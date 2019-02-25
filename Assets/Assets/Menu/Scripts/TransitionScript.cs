using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour {

    public void changinScene()
    {
            SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
            Application.Quit();
    }
}
