using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum Difficulty
{
    Easy,
    Medium,
    Hard
}


public class SpawnBehaviour : MonoBehaviour
{
    [SerializeField] GameObject spawnOrigin;
    [SerializeField] GameObject duckBlack;
    [SerializeField] GameObject duckGreen;
    [SerializeField] GameObject duckPink;
    [SerializeField] GameObject readyUi;
    [SerializeField] TextMeshProUGUI readyText;


    [SerializeField] private List<GameObject> listOfDucks = new List<GameObject>(0);


    public Difficulty difficulty;

    const int easyLimit = 1;
    const int midLimit = 3;
    const int hardLimit = 5;

    [SerializeField] int levelCounter = 10;
    [SerializeField] int enemyCounter = 0;
    [SerializeField] int enemiesToSpawn = 0;
    [SerializeField] bool isSpawningEnemies;
    [SerializeField] int timerSeconds = 1;
    [SerializeField] int timeUntilNextRound = 0;


    private static SpawnBehaviour _instance;
    public static SpawnBehaviour Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Spawnmanager is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {

        timerSeconds = 1;
        TrackDifficulty();
        MakeNewDuckList();
        enemiesToSpawn = levelCounter;
        isSpawningEnemies = true;
        //StartNextDuck();
        StartCoroutine(StartMatch(4));
        StartCoroutine(ShowReadyText());

    }


    //the base function to spawn an enemy
    void SpawnEnemy()
    {
        enemyCounter++;
        isSpawningEnemies = false;

        //Find a random enemy type
        int myenemy = Random.Range(0, listOfDucks.Count);


        switch (difficulty)
        {


            case Difficulty.Easy:

                if (listOfDucks.Count == 0)
                {
                    break;
                }

                Instantiate(listOfDucks[myenemy], spawnOrigin.transform.position, spawnOrigin.transform.rotation);
                listOfDucks.Remove(listOfDucks[myenemy]);//remove the index to prevent spawning the same one
                Debug.Log("Spawning easy ducks");
                break;

            case Difficulty.Medium:

                if (listOfDucks.Count == 0)
                {
                    break;
                }

                Instantiate(listOfDucks[myenemy], spawnOrigin.transform.position, spawnOrigin.transform.rotation);
                listOfDucks.Remove(listOfDucks[myenemy]);//remove the index to prevent spawning the same one
                Debug.Log("Spawning medium ducks");
                break;

            case Difficulty.Hard:

                if (listOfDucks.Count == 0)
                {
                    break;
                }

                Instantiate(listOfDucks[myenemy], spawnOrigin.transform.position, spawnOrigin.transform.rotation);
                listOfDucks.Remove(listOfDucks[myenemy]);//remove the index to prevent spawning the same one
                Debug.Log("Spawning hard ducks");
                break;


            default:
                Debug.Log("Incorrect duck range");
                break;

        }

    }

    //adds pace and controls how long the player waits per duck and per round
    IEnumerator StartSpawnTimer(float waitTime)
    {
        Debug.Log("starting timer");

        while (isSpawningEnemies == true)
        {
            TrackDifficulty();
           

            //if the round timer has been set then wait for the roundtimer, else spawn each enemy normally
            if (timeUntilNextRound > 0)
            {

                Debug.Log("starting next round");

                //TODO: add a visual counter to the ui
               
                SetEnemiesLeft(GetLevelCounter());
                ResetEnemyCounter();
               


                yield return new WaitForSeconds(timeUntilNextRound);

                GameManager.Instance.IncreaseRound();
                GameManager.Instance.ResetDucksHit();
                timeUntilNextRound = 0;

                if (enemyCounter != levelCounter)
                {
                    Debug.Log("spawning start round");
                    yield return new WaitForSeconds(waitTime);
                    SpawnEnemy();


                }
            }
            else
            {
                if (enemyCounter != levelCounter)
                {
                    Debug.Log("spawning normally");
                    yield return new WaitForSeconds(waitTime);
                    SpawnEnemy();

                }


            }



        }

        //Debug.Log("check is spawning enemies");

    }

    IEnumerator StartMatch(float waitTime)
    {
        Debug.Log("starting match");

        if (enemyCounter != levelCounter)
        {
            Debug.Log("spawning normally");
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();

        }
    }

    IEnumerator ShowReadyText()
    {
       
        readyUi.SetActive(true);

        yield return new WaitForSeconds(2);

        readyText.text = "GO!";

        yield return new WaitForSeconds(1);

        readyUi.SetActive(false);


    }

    public void StartNextDuck() { StartCoroutine(StartSpawnTimer(timerSeconds)); }
    public int GetEnemyCounter() { return enemyCounter; }

    public void ResetEnemyCounter() { enemyCounter = 0; }
    public int GetLevelCounter() { return levelCounter; }
    public void SetSpawning(bool isSpawn) { isSpawningEnemies = isSpawn; }
    public void SetNextRoundTimer(int amount) { timeUntilNextRound += amount; }
    public void UpdateEnemiesLeft() { enemiesToSpawn--; }
    public void SetEnemiesLeft(int amount) { enemiesToSpawn = amount; }
    public int GetEnemiesLeft() { return enemiesToSpawn; }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        listOfDucks.Remove(enemy);
    }


    void TrackDifficulty()
    {
        if (GameManager.Instance.GetRounds() == easyLimit)
        {
            difficulty = Difficulty.Easy;
            //Debug.Log("the round is now easy");
        }
        if (GameManager.Instance.GetRounds() == midLimit)
        {
            difficulty = Difficulty.Medium;
            //Debug.Log("the round is now medium");
        }
        if (GameManager.Instance.GetRounds() == hardLimit)
        {
            difficulty = Difficulty.Hard;
            //Debug.Log("the round is now hard");
        }

    }


    List<GameObject> GenerateDucks(GameObject ducktToAdd, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            listOfDucks.Add(ducktToAdd);

        }

        //update the list
        return listOfDucks;
    }

    public void MakeNewDuckList()
    {
        switch (difficulty)
        {

            case Difficulty.Easy:


                GenerateDucks(duckBlack, 10);
                Debug.Log("Made easy duck list");
                break;

            case Difficulty.Medium:

                GenerateDucks(duckBlack, 5);
                GenerateDucks(duckGreen, 5);

                Debug.Log("Made medium duck list");
                break;

            case Difficulty.Hard:

                GenerateDucks(duckBlack, 3);
                GenerateDucks(duckGreen, 3);
                GenerateDucks(duckPink, 4);

                Debug.Log("Made hard duck list");
                break;


            default:
                Debug.Log("Incorrect duck range");
                break;

        }
    }

   

}

