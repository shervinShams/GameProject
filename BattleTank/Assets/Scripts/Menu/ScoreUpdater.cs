using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    public GameObject score;
    public GameManager gameManager;
    public int playerNumber = 1;
    //PlayerPrefs playerPrefs;
   // int playerScore;

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        //playerPrefs = GetComponent<PlayerPrefs>();
        // playerPrefs = GetComponent<PlayerPrefs>();
       
        //DontDestroyOnLoad(score);
    }

    void FixedUpdate()
    {
        // PlayerPrefs.SetInt("Score", gameManager.GetScoreForPlayer(playerNumber));
         //playerScore = gameManager.GetScoreForPlayer(playerNumber);
        //PlayerPrefs.SetInt("Score", playerScore);
        if (playerNumber == 1)
        {
           text.text = "Score: " + PlayerPrefs.GetInt("Score1" , 0);
            Debug.Log(PlayerPrefs.GetInt("Score1"));
        }

        if (playerNumber == 2)
        {
            text.text = "Score: " + PlayerPrefs.GetInt("Score2" , 0);
            Debug.Log(PlayerPrefs.GetInt("Score2"));

        }

        //PlayerPrefs.Save();

    }
}
