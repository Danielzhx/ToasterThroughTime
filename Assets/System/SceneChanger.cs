using UnityEngine;
using UnityEngine.SceneManagement;

namespace TTT.System
{
    public class SceneChanger : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        public void Quit()
        {
            Application.Quit();
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public void RestartLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
        }
    }
}

