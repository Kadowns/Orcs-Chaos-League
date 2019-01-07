
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.Menu {
    public class TrasitionToScene : MonoBehaviour{


        public void GoToScene(int index) {
            SceneManager.LoadScene(index);
        }
    }
}