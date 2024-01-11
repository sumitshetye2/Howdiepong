using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
  private int _baseScore = 10;
  private int _scoreMultiplier = 1;
  private int _totalScore = 0;
  private int _highScore = 0;

  private int _previousScoreIncreament = 0;
  private int _scoreDelta = 100;

  public int scoreDelta
  {
    get { return _scoreDelta; }
    set { _scoreDelta = value; }
  }

  public int HighScore { get { _highScore = PlayerPrefs.GetInt("HighScore", 0); return _highScore; } 
    set { _highScore = value; if (PlayerPrefs.GetInt("HighScore", 0) < value) { PlayerPrefs.SetInt("HighScore", value); } } }

  public void ResetScoreModule()
  {
    _baseScore = 10;
    _scoreMultiplier = 1;
    _totalScore = 0;
    _previousScoreIncreament = 0;    
  }



  public void IncreasesScore()
  {
    _totalScore = _totalScore + (_baseScore * _scoreMultiplier);
    //Debug.Log("Score : " + _totalScore + " : _baseScore : " + _baseScore + " : Score Multiplier : " + _scoreMultiplier);
  }

  public void IncreamentScoreMultiplier()
  {
    _scoreMultiplier += 1;
  }


  public void ResetScoreMultiplier()
  {
    _scoreMultiplier = 1;
  }

  public int GetScoreMultiplier()
  {
    return _scoreMultiplier;
  }

  public int GetScore()
  {
    return _totalScore;
  }
  public void ResetHighScore()
  {
    _highScore = 0;
  }



  public bool IsSpeedincreamentRequired()
  {
    bool required = false;

    if (_totalScore - _previousScoreIncreament >= _scoreDelta)
    {
      required = true;
      _previousScoreIncreament = _totalScore;
    }

    return required;
  }
}
