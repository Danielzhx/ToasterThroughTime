using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    public void StartGame(){
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void Quit(){
        Application.Quit();
    }
}
