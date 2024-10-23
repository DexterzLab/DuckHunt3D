using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Turning the game manager into a singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Gamemanager is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private int score = 0;
    [SerializeField] private int Rounds = 1;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI roundsText;
    [SerializeField] TextMeshProUGUI hitDucksText;
    [SerializeField] TextMeshProUGUI missedDucksText;
    [SerializeField] GameObject gameUiCanvas;
    [SerializeField] GameObject gameOverCanvas;

    public bool isGameActive;

    //int ducksToSpawn;
    int hitDucks = 0;
    int missedDucks = 0;


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
        roundsText.text = Rounds.ToString();
        hitDucksText.text = hitDucks.ToString() + "/ 10";//hard coded value as there will only be 10 ducks
        missedDucksText.text = missedDucks.ToString() + "/ 4";
        
        isGameActive = true;
    }

  

    public int GetMissedDuck() { return missedDucks; }
    public int GetScore() { return score; }
    public int GetRounds() { return Rounds; }

    public void AddMissedDuck() 
    { 
        missedDucks++;
        missedDucksText.text = missedDucks.ToString() + "/ 4";
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

    public void UpdateDucksHit(int ducksToAdd)
    {
        hitDucks += ducksToAdd;
        hitDucksText.text = hitDucks.ToString() + "/ 10";
    }

    public void ResetDucksHit()
    {
        hitDucks = 0;
        hitDucksText.text = hitDucks.ToString() + "/ 10";

    }
    public void IncreaseRound() 
    { 
        Rounds++;
        roundsText.text = Rounds.ToString();
        SpawnBehaviour.Instance.MakeNewDuckList();
    }


    public void CallGameOver()
    {
        isGameActive = false;
        StartCoroutine(PerformGameOverTasks());
        //Debug.Log("performing game over");
    }

    IEnumerator PerformGameOverTasks()
    {
        //Ui and other tasks
        gameUiCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);

        yield return new WaitForSeconds(3);


        //change level
        SceneManager.LoadScene(sceneBuildIndex: 0);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
