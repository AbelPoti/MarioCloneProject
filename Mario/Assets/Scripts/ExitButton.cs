using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    GameObject pauseCanvas;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        GameController.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameController.Instance.ContinueGame();
    }
}
