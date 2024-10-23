using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(0, 10)][SerializeField] protected float Speed;
    [SerializeField] protected int baseScore;
    [SerializeField] protected int additionalPoints;
    [SerializeField] protected int currentScore;
    [SerializeField] protected float scoreMultiplier;
    [SerializeField] protected int bonusPointsReduction;

    private float timeAlive;
    private float timeWhenDecreased;

    //[SerializeField] private Transform[] flightTarget = new Transform[8];

    //Constructor
    public Enemy()
    {
        Speed = 5;
        baseScore = 100;
        additionalPoints = 100;
        currentScore = baseScore + additionalPoints;
        bonusPointsReduction = 10;
        scoreMultiplier = 1;
        timeAlive = 1;
        timeWhenDecreased = 0;
    }

    //destructor
    ~Enemy()
    {


       
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        currentScore = Mathf.Clamp(currentScore, baseScore, baseScore+additionalPoints);

        DecreaseBonusPoints();
        
    }

    void DecreaseBonusPoints()
    {

        if(Time.time - timeAlive > timeWhenDecreased)
        {
            timeWhenDecreased = Time.time;
            currentScore -= bonusPointsReduction;
       

            //Debug.Log("Points are now: " + currentScore);
        }
    }

    public int GetPoints() { return currentScore; }

    public float GetSpeed() { return Speed; }

 
    

   







}
