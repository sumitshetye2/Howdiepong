using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _scoreText;
  [SerializeField] private TextMeshProUGUI _highScoreText;
  [SerializeField] private TextMeshProUGUI _multiplierText;
  [SerializeField] private TextMeshProUGUI _livesText;
  [SerializeField] private GameObject _gameOverPanel;
  [SerializeField] private GameObject _gameHUDPanel;
  [SerializeField] private GameObject _gameStartPanel;
  [SerializeField] private TextMeshProUGUI _10SecondTimer;
  [SerializeField] private TextMeshProUGUI _endGameScore;


  private void OnEnable()
  {
    Timer.uiUpdateEvent += UpdateSlider;

  }

  public void SetScoreToUI(int score)
  {
    _scoreText.text = "Score: " + score.ToString();
  }

  public void SetHighScoreToUI(int highScore)
  {
    _highScoreText.gameObject.SetActive(true);
    _highScoreText.text = "HighScore: " + highScore.ToString();
  }

  public void SetScoreMultiplierToUI(int multiplier)
  {
    _multiplierText.text = "Multiplier: " + multiplier;
  }

  public void DecreaseLives(int currentLives)
  {
    _livesText.text = "Lives: " + currentLives.ToString();
  }


  public void PanelActive()
  {
    _gameHUDPanel.SetActive(false);
    _gameOverPanel.SetActive(true);
  }

  public void StartPanelActive()
  {
    _gameHUDPanel.SetActive(false);
    _gameOverPanel.SetActive(false);
    _gameStartPanel.SetActive(true);
  }

  public void OnClickStartButton()
  {
    GameManager.instance.StartGameSubSystems();
    _gameStartPanel.SetActive(false);
    _gameHUDPanel.SetActive(true);
  }
  public void GameOver()
  {
    _gameHUDPanel.SetActive(false);
    _gameOverPanel.SetActive(true);
    _scoreText.text = "Score: 0";
    _livesText.text = "Lives: 3";
    _multiplierText.text = "Multiplier: 1";
    _10SecondTimer.gameObject.SetActive(false);
  }

  public void SetScoreAtGameEnd(int score)
  {
    _endGameScore.text = "Score: " + score;
  }

  public void GameStart()
  {
    _gameHUDPanel.SetActive(true);
    _gameOverPanel.SetActive(false);
    _10SecondTimer.gameObject.SetActive(true);
  }

  public void OnClickRestartButton()
  {
    GameManager.instance.StartGameSubSystems();
  }

  public void OnCLickMainMenu()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
  }

  void UpdateSlider (int time)
  {
    //float sliderValue = (float)time / 10f;
    _10SecondTimer.text = (11 - time).ToString();
    //Debug.Log("Slider value : " + _10SecondTimer.value);
  }

  public void OnClickQuit()
  {
    Application.Quit();
  }

  private void OnDisable()
  {
    Timer.uiUpdateEvent -= UpdateSlider;
  }

}
