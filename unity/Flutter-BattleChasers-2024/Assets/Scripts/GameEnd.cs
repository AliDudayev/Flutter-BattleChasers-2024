using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FlutterUnityIntegration;
using System;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    private int endScore;
    private List<string> killedDragons = new List<string>(); // Store unique dragon IDs
    public int killCount;

    public Text debug;

    private int buttonPressed = 0;

    //public void ExitGame()
    //{
    //    //Application.Quit();
    //    SendResultsToFlutter();
    //    debug.text = "Exit game triggered";
    //}
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

    public void IncreaseKillCount(){
        //Console.WriteLine("KillCount:" + killCount);

        //int killCountInt = int.Parse(killCount);
        //killCountInt++;
        //this.killCount = killCountInt.ToString();
        killCount++;
    }

    public void AddKilledDragon(string dragonID)
    {
        if (!string.IsNullOrEmpty(dragonID))
        {
            killedDragons.Add(dragonID);
        }
    }

    // Method to get the list of killed dragons
    public List<string> GetKilledDragons()
    {
        return new List<string>(killedDragons);
    }


    // Send the score and killed dragons to Flutter
    public void SendResultsToFlutter()
    {
        // Create a data object to serialize
        debug.text += "\nSendResultsToFlutter entered";

        var results = new GameResults
        {
            score = endScore,
            killedDragons = new List<string>(killedDragons),
            count = killCount.ToString()
        };

        // Convert to JSON
        string jsonResults = JsonUtility.ToJson(results);

        // Debug log the result
        debug.text += "\nSending results to Flutter: " + jsonResults;

        debug.text += "\nKilledDragons: " + string.Join(", ", killedDragons) + "\nEndscore: " + endScore + "\ncount: " + killCount;
        if(buttonPressed < 1)
        {
            buttonPressed++;
            return;
        }
        

        // Send the JSON to Flutter
        UnityMessageManager.Instance.SendMessageToFlutter(jsonResults);
    }

    [Serializable]
    public class GameResults
    {
        public int score;
        public List<string> killedDragons;
        public string count;
    }

}