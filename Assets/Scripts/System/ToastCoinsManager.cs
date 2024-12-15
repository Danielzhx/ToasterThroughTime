using TMPro;
using UnityEngine;

public class ToastCoinsManager : MonoBehaviour
{
    public static ToastCoinsManager instance;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pauseMenuScoretext;

    void Awake()
    {
        //Set this to be the singleton isntance 
        instance = this;
    }
    void Update()
    {
        scoreText.text = score.ToString();
        pauseMenuScoretext.text = score.ToString();
    }
}
