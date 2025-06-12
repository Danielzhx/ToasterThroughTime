using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenuPanel;
    public GameObject deathScreenPanel;
    public GameObject winScreenPanel;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                return;
            }
            else
            {
                PauseGame();
                return;
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        deathScreenPanel.SetActive(false);
        winScreenPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        deathScreenPanel.SetActive(true);
        winScreenPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
