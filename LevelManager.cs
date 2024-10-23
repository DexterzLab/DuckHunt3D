using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private string highScoreKey = "Highscore";
    public TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Start()
    {
        highscoreText.text = PlayerPrefs.GetInt(highScoreKey, 0).ToString();
            
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
