using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdateSnow : MonoBehaviour {

    public GameManagerSnow gameManager;
    public int playerNumber = 1;
   // public GameObject score;
  // public  PlayerPrefs playerScore;
  //  int playerScore;
   
    private Text text;

    // void Start()
    //{
    //    playerScore = PlayerPrefs.GetInt("Score");
    //}

    void Awake()
    {
        
        text = GetComponent<Text>();
       // score = GetComponent<GameObject>();
       // playerScore = GetComponent<PlayerPrefs>();
       // DontDestroyOnLoad(score);

    }

    void FixedUpdate()
    {
       // playerScore = gameManager.GetScoreForPlayer(playerNumber);
        //text.text = "Score: " + playerScore;
        //PlayerPrefs.GetInt("Score", playerScore);

        if (playerNumber == 1)
        {
            text.text = "Score: " + PlayerPrefs.GetInt("Score1");
        }

        if (playerNumber == 2)
        {
            text.text = "Score: " + PlayerPrefs.GetInt("Score2");
        }

    }
}
