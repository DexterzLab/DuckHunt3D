using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> trackedTarget = new List<GameObject>(8);
    [SerializeField] private GameObject[] flightTarget;
    [SerializeField] private GameObject originSpawnPoint;

    //the number of times the duck will move
    [SerializeField]int timesCanMove = 5;
    [SerializeField]int timesMoved = 0;
    //[Range(0, 10)][SerializeField]float speed = 20;
    [SerializeField]int waypointIndex = 0;
    [SerializeField]bool moveToSpawn = false;
    [SerializeField]Enemy self;
    int nextIndex = 0;


    private void Awake()
    {
        self = GetComponent<Enemy>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        FindFlightTargets();
        waypointIndex = Random.Range(0, flightTarget.Length);
        nextIndex = Random.Range(0, flightTarget.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTheDuck();
    }

    void FindFlightTargets()
    {
        if(flightTarget.Length == 0)
        {
            //Debug.Log("Trying to find targets");
            flightTarget = GameObject.FindGameObjectsWithTag("FlightTarget");
        }
            
     

        if (originSpawnPoint == null)
        {
            //Debug.Log("Trying to find origing");
            originSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        }

        
    }

    void MoveTheDuck()
    {
        Vector3 destination = flightTarget[waypointIndex].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, self.GetSpeed() * Time.deltaTime);//a variable to store the move towards function
        float distance = Vector3.Distance(transform.position, destination);

        //if we have reached our target go to a new random waypoint
        if(moveToSpawn == false)
        {
            //use the current assigned destination
            transform.position = newPos;

            if (distance <= 0.05 && timesMoved != timesCanMove) {

                waypointIndex = Random.Range(0, flightTarget.Length);
                nextIndex = Random.Range(0, flightTarget.Length);

                if(waypointIndex == nextIndex)
                {
                    waypointIndex = Random.Range(0, flightTarget.Length);
                    destination = flightTarget[nextIndex].transform.position;

                    Debug.Log("moving to correction");
                }




                timesMoved++;
                
                Debug.Log("moving to next point  ");
                
                if(timesMoved >= timesCanMove)
                {
                    moveToSpawn = true;
                }
                
               
            }
        }
        else if (moveToSpawn == true)
        {
            //ignore the array and travel to the spawn
            newPos = Vector3.MoveTowards(transform.position, originSpawnPoint.transform.position, self.GetSpeed() * Time.deltaTime);//a variable to store the move towards function
            transform.position = newPos;

        }

        if(moveToSpawn == true && transform.position == originSpawnPoint.transform.position)
        {
            Destroy(gameObject, 1f);
           
        }
        






    }

    private void OnDestroy()
    {
        //SpawnBehaviour.Instance.RemoveEnemyFromList(myEnemy.gameObject);
        SpawnBehaviour.Instance.UpdateEnemiesLeft();

        //once player has killed all ten enemies, tell the spawn manager to 
        if (SpawnBehaviour.Instance.GetEnemiesLeft() == 0)
        {
            SpawnBehaviour.Instance.SetNextRoundTimer(5);
            
            SpawnBehaviour.Instance.SetSpawning(true);
            SpawnBehaviour.Instance.StartNextDuck();
        }
        else if (SpawnBehaviour.Instance.GetEnemyCounter() != SpawnBehaviour.Instance.GetLevelCounter())
        {
            //part of the system so that we can spawn the duck one at a time
            SpawnBehaviour.Instance.SetSpawning(true);
            SpawnBehaviour.Instance.StartNextDuck();
        }

        if (GameManager.Instance.GetMissedDuck() >= 3 && GameManager.Instance.isGameActive == true)
        {
            GameManager.Instance.CallGameOver();
            // Debug.Log("gameover");

        }


        //if destroy while at spawn, call a miss
        if (moveToSpawn == true && transform.position == originSpawnPoint.transform.position)
        {
            GameManager.Instance.AddMissedDuck();

        }
    }

    /* basic movement function
    void Move()
    {
        if(transform.position != Target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
        }
       
    }
    */

}
