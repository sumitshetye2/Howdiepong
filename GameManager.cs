using Mediapipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public delegate void SetMotionDetails(float speed, float accelaration, float maxSpeed);
  public static SetMotionDetails setMotionDetails;

  [SerializeField] private GameObject _ballPrefab;
  [SerializeField] private GameObject _asteroidPrefab;
  [SerializeField] private GameObject[] _asteroidPrefabs;
  [SerializeField] private Transform _originalBall;

  private Vector3 _ballInstatntiatePosition = new Vector3();
  //[SerializeField] private Transform _directionVectorParent;

  private Vector3 _asteroidInstantiatePosition = new Vector3();
  [SerializeField] private Transform _directionVectorParent;
  private float startDelay = 1.25f;
  private float spawnInterval = .75f;

  private List<GameObject> _currentBalls = new List<GameObject>();
  private ScoringManager _scoreManager;

  [SerializeField] private UIManager _uiManager;
  private GameObject _paddle;

  private float _speed = 20f;
  private float _accelaration = 5;
  private float _maxSpeed = 35f;

  private int _maxLives = 3;
  private int _currentLives;

  private float initialCountdownTime = 10.0f;
  private float currentCountdownTime;
  private bool isTimerRunning = false;

  private float gameOverInitCountTime = 1.5f;
  private float currentGameOverTime;
  private bool isGameOverCountRunning = false;

  public static GameManager instance;
  public bool isGameRunning { get; private set; }

  public AnimationCurve curve;

  private void Awake()
  {
    Timer.every10Seconds += TenSecondChange;
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);

    _asteroidInstantiatePosition = _directionVectorParent.position;
    _currentBalls.Add(GameObject.FindObjectOfType<BallController>().gameObject);

    _scoreManager = GetComponent<ScoringManager>();

    _currentLives = _maxLives;
  }

  private void OnEnable()
  {
    _paddle = GameObject.FindGameObjectWithTag("Player");
    isGameRunning = false;
  }

  private void Start()
  {
    UseConfiguration();
    setMotionDetails?.Invoke(_speed, _accelaration, _maxSpeed);
    _scoreManager.ResetHighScore();
    _uiManager.SetHighScoreToUI(0);
    _paddle.SetActive(false);
    _uiManager.StartPanelActive();
    Debug.Log("Starting multiplier is: " + _scoreManager.GetScoreMultiplier());

    //InvokeRepeating("SpawnAsteroid", startDelay, spawnInterval);
  }

  private void ResetTimer()
  {
    isTimerRunning = false;
    currentCountdownTime = initialCountdownTime;
  }

  private void StartTimer()
  {
    isTimerRunning = true;
  }

  private void ResetGameOverTimer()
  {
    isGameOverCountRunning = false;
    currentGameOverTime = gameOverInitCountTime;
  }

  private void StartGameOverCountdown()
  {
    isGameOverCountRunning = true;
  }

  private void TenSecondChange()
  {

    //_scoreManager.IncreamentScoreMultiplier();
    //_uiManager.SetScoreMultiplierToUI(_scoreManager.GetScoreMultiplier());
    //Debug.Log("Ten seconds are done.. Instantiate another ball");
    GameObject newBall = GetBall();


    float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y-5f, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y+5f);
    float spawnX = Random.Range
        (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 5f, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 5f);

    /*float spawnY = Random.Range(0f, Screen.width);
    float spawnX = Random.Range(0f, Screen.height);*/

    /*float spawnY = Random.Range(-5f, 5f);
    float spawnX = Random.Range(-5f, 5f);*/

    Vector3 spawnPosition = new Vector3(spawnX, spawnY, _ballInstatntiatePosition.z-150f);

    Debug.Log("spawnPosition "+ spawnPosition);
    Debug.Log("_ballInstatntiatePosition " + _ballInstatntiatePosition);

    if (newBall == null)
    {
      newBall = Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
      _currentBalls.Add(newBall);
    }
    newBall.transform.position = spawnPosition;
    var ballController = newBall.GetComponent<BallController>();
    ballController.SetMotionDetails(_speed, _accelaration, _maxSpeed);
    ballController.InitialPush();
    newBall.SetActive(true);
  }

  private GameObject GetBall()
  {
    GameObject toReturn = null;
    foreach (GameObject go in _currentBalls)
    {
      if(go.activeSelf == false)
      {
        toReturn = go;
        break;
      }
    }
    return toReturn;
  }

  public void SpawnAsteroid()
  {
    if(isGameRunning == true)
    {
      float spawnY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y - 100f, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 100f);
      float spawnX = Random.Range
                  (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 100f, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 100f);

      Vector3 spawnPosition = new Vector3(spawnX, spawnY, _asteroidInstantiatePosition.z + 150f);
      int index = Random.Range(0, 2);
      Instantiate(_asteroidPrefabs[index], spawnPosition, _asteroidPrefabs[index].transform.rotation);
    }

    else
    {
      CancelInvoke("SpawnAsteroid");
    }


    //Debug.Log("Asteroid spawned!");
  }






  public int GetLives()
  {
    return _currentLives;
  }

  public void ReduceLives()
  {
    _currentLives--;
  }

  public Transform GetAllPossibleDirections()
  {
    return _directionVectorParent;
  }

  public void BallHit()
  {
    _scoreManager.IncreasesScore();
    _scoreManager.IncreamentScoreMultiplier();
    _uiManager.SetScoreToUI(_scoreManager.GetScore());
    _uiManager.SetScoreMultiplierToUI(_scoreManager.GetScoreMultiplier());
    IncreaseSpeedWhenNeeded();
  }
  public void Update()
  {
    if (isGameRunning)
    {
      if(_currentLives > 0)
      {
        _scoreManager.IncreasesScore();
        _uiManager.SetScoreToUI(_scoreManager.GetScore());
      }
      
      if (!isTimerRunning)
      {
        StartTimer();
      }
      currentCountdownTime -= Time.deltaTime;
      if (currentCountdownTime <= 0)
      {
        ResetTimer();
        _scoreManager.IncreamentScoreMultiplier();
        _uiManager.SetScoreMultiplierToUI(_scoreManager.GetScoreMultiplier());
      }

      CheckForGameOver();
      //_uiManager.DecreaseLives();
    }
    else
    {
      ResetTimer();
    }

 
  }
  public void BallMissed()
  {
    _scoreManager.ResetScoreMultiplier();
    _uiManager.SetScoreMultiplierToUI(_scoreManager.GetScoreMultiplier());
    CheckForGameOver();
  }

  private void CheckForGameOver()
  {
    //bool shouldTriggerGameOver = true;
    /*
    foreach (var ball in _currentBalls)
    {
      if (ball.activeSelf)
      {
        shouldTriggerGameOver = false;
        break;
      }
    }
    */
    if (_currentLives == 0)
    {
      //want to game over after delay so last explosion plays
      // we will call the game over screen here
      _uiManager.SetScoreAtGameEnd(_scoreManager.GetScore());
      Invoke("SetPaddleInactive", 0.2f);
      Invoke("CallGameOver", 1.0f);
      isGameRunning = false;
      _scoreManager.HighScore = _scoreManager.GetScore();
      _uiManager.SetHighScoreToUI(_scoreManager.HighScore);
      //reset score multiplier
      _scoreManager.ResetScoreMultiplier();
     
    }
  }

  private void CallGameOver()
  {
    _uiManager.GameOver();
  }
  private void SetPaddleInactive()
  {
    _paddle.SetActive(false);
  }

  public void StartGameSubSystems()
  {
    UseConfiguration();
    _scoreManager.ResetScoreModule();
    _uiManager.SetScoreMultiplierToUI(_scoreManager.GetScoreMultiplier());
    _uiManager.GameStart();
    DestroyAsteroids();
    _paddle.SetActive(true);
    _currentLives = _maxLives;
    isGameRunning = true;
    InvokeRepeating("SpawnAsteroid", startDelay, spawnInterval);
    TenSecondChange();
  }

  private void UseConfiguration()
  {
    if (ConfigurationObject.instance == null)
    {
      return;
    }
    _speed = ConfigurationObject.instance.speed;
    _accelaration = ConfigurationObject.instance.accelaration;
    _maxSpeed = ConfigurationObject.instance.topSpeed;
    _scoreManager.scoreDelta = ConfigurationObject.instance.acclTrigger;
    //_currentLives = 3;
  }

  public void DestroyAsteroids()
  {
    GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
    foreach (GameObject asteroid in asteroids)
    {
      Destroy(asteroid);
    }
  }
  private void IncreaseSpeedWhenNeeded()
  {
    if (_scoreManager.IsSpeedincreamentRequired())
    {
      //_speed += _accelaration;
      //if (_speed >= _maxSpeed)
      //  _speed = _maxSpeed;
      float from = _speed;
      float to = (from + _accelaration) > _maxSpeed ?
        _maxSpeed : (from + _accelaration);

      StartCoroutine(SmoothIncreament(from, to, 1f));
    }
  }

  public float GetSpeed() { return _speed; }
  public float GetAccelaration() { return _accelaration; }
  public float GetMaxSpeed() { return _maxSpeed; }

  public IEnumerator SmoothIncreament(float from, float to, float timeToMove)
  {
    if (from != to)
    {
      var t = 0f;
      while (t < 1)
      {
        t += Time.deltaTime / timeToMove;
        _speed = Mathf.Lerp(from, to, curve.Evaluate(t));
        setMotionDetails?.Invoke(_speed, _accelaration, _maxSpeed);
        yield return null;
      }
    }
  }

  private void OnDisable()
  {
    Timer.every10Seconds -= TenSecondChange;
  }




}
