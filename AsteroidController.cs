using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AsteroidController : MonoBehaviour
{
  private Transform _asteroidTransform;
  private Vector3 _initialDirection;

  private UIManager uiManager;
  [SerializeField] private GameManager gameManager;
  private ScoringManager scoreManager;




  [SerializeField] private float speed;
  // Start is called before the first frame update
  private void Start()
  {
    uiManager = GameObject.Find("Game_Canvas").GetComponent<UIManager>();
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    scoreManager = GameObject.Find("GameManager").GetComponent<ScoringManager>();
    //Get transform components of the asteroid and the main camera
    _asteroidTransform = transform;

    RecordInitialDirection();
  }

  private void RecordInitialDirection()
  {
    float destX = Random.Range(Camera.main.transform.position.x - 5f, Camera.main.transform.position.x + 5f);
    float destY = Random.Range(Camera.main.transform.position.y - 5f, Camera.main.transform.position.y + 5f);
    float destZ = Camera.main.transform.position.z;
    Vector3 positionToTravel = new Vector3(destX, destY, destZ);
    _initialDirection = (positionToTravel - _asteroidTransform.position).normalized;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      Destroy(gameObject);
      int lives = gameManager.GetLives();
      lives--;
      uiManager.DecreaseLives(lives);
      gameManager.ReduceLives();
      /*
      if (lives == 0)
      {
        uiManager.GameOver();
        uiManager.SetScoreAtGameEnd(scoreManager.GetScore());
      }
      */
    }
    
  }



  // Update is called once per frame
  void Update()
  {



    _asteroidTransform.Translate(speed * Time.deltaTime * _initialDirection);

    if(gameObject.transform.position.z < 75)
    {
      Destroy(gameObject);
    }



    //handle destruction of asteroid if goes past camera


  }
}
