using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI bulletsText;
    [SerializeField] int Bullets = 3;
    [SerializeField] Camera selfCamera;

    [Range(0, 1)] [SerializeField] float aimAssistSize = 0.5f;
    AudioSource myAudioSource;
    [SerializeField]AudioClip shootAudio;
    [SerializeField]AudioClip reloadAudio;

    Vector3 mousePos;
    Touch touch;//variable for the phone touches

    Vector3 screenPosition;//start point of ray
    Vector3 worldPosition;//endpoint of ray

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        mousePos = Input.mousePosition;
        selfCamera = Camera.main;
        bulletsText.text = Bullets.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        //MappingMouse();
        //MouseControls();


        TouchControls();

    }

   

    void TouchControls()
    {

        //track how many bullets we have, if it hits zero then its game over
        if (Bullets == 0 && GameManager.Instance.isGameActive == true)
        {
            GameManager.Instance.CallGameOver();
           // Debug.Log("gameover");

        }


        if (Input.touchCount > 0 && GameManager.Instance.isGameActive == true)
        {
            //we only want to register the first finger that touches the screen
            touch = Input.GetTouch(0);

            //only perform 1 call at the beginning of the touch
            if (touch.phase == TouchPhase.Began)
            {
                myAudioSource.PlayOneShot(shootAudio);
                //Debug.Log("you shot a bullet");

                //calculate the end point
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                RemoveBullet();

                if (Physics.SphereCast(ray, aimAssistSize, out RaycastHit hitinfo, Mathf.Infinity))
                {
                    CheckForDuckType(hitinfo);

                   // Debug.Log("You have shot: " + hitinfo.collider.name);
                   
                }

                Debug.DrawRay(ray.origin,ray.direction*20);
                //Debug.Log("The touch count is: " + Input.touchCount);
            }


        }
    }



    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, aimAssistSize);
    }

    

    bool CheckForDuckType(RaycastHit HitInfo)
    {

        //TODO: change these to layermasks for further polish


        Enemy myEnemy = HitInfo.collider.GetComponent<Enemy>();

        

        if (HitInfo.collider.CompareTag("Duck"))
        {
            GameManager.Instance.UpdateDucksHit(1);
            GameManager.Instance.UpdateScore(myEnemy.GetPoints());
            Destroy(HitInfo.collider.gameObject);
            AddBullet();

            if(GameManager.Instance.GetScore() > PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore", GameManager.Instance.GetScore());
            }

            

            //once player has killed all ten enemies, tell the spawn manager to 
            if (SpawnBehaviour.Instance.GetEnemiesLeft() == 0)
            {
                myAudioSource.PlayOneShot(reloadAudio);
                ResetBullets();    
            }

            //Debug.Log("Enemy has awarded points");

            return true;
        }



        
        return false;
    }


    
    void RemoveBullet()
    {    
        Bullets--;
        bulletsText.text = Bullets.ToString();
    }

    void AddBullet()
    {
        Bullets++;
        bulletsText.text = Bullets.ToString();
    }

    void ResetBullets()
    {
        Bullets = 3;
        bulletsText.text = Bullets.ToString();
    }






    void MappingMouse()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 100f;

        worldPosition = selfCamera.ScreenToWorldPoint(screenPosition);

        //Debug.Log(worldPosition);
        Debug.DrawRay(transform.position, worldPosition - transform.position, Color.green);
    }


    void MouseControls()
    {

        //track how many bullets we have, if it hits zero then its game over
        if (Bullets == 0 && GameManager.Instance.isGameActive == true)
        {
            GameManager.Instance.CallGameOver();
            // Debug.Log("gameover");

        }


        if (Input.GetMouseButtonDown(0) && GameManager.Instance.isGameActive == true)
        {
         
            screenPosition = Input.mousePosition;

            //calculate the end point
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            RemoveBullet();

            if (Physics.SphereCast(ray, aimAssistSize, out RaycastHit hitinfo, Mathf.Infinity))
            {
                CheckForDuckType(hitinfo);

                // Debug.Log("You have shot: " + hitinfo.collider.name);

            }

            //Debug.Log("You have fired the gun");

        }




    }





}
