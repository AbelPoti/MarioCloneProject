using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void PlayGame()
    {
        try
        {
            //GameController.Instance.LoadNextLevel();
            SceneManager.LoadScene("1 - 1");
        }
        catch(Exception)
        {
            Debug.Log("GameController is null");
        }
    }

    public void LogSmth()
    {
        Debug.Log("Hovered over me");
    }
}
