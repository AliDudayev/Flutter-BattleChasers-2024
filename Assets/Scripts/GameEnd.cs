using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    private int endScore;
    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetEndScore(int newEndScore)
    {
        endScore = newEndScore;
    }
    public int GetEndScore()
    {
        return endScore;
    }
}
