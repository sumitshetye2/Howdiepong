using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
  public void OnClickStartButton()
  {
    SceneManager.LoadScene(1);
  }

  public void OnCLickQuit()
  {
    Application.Quit();
  }
}
