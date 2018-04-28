using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
   
    public int playerNumber = 1;
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void FixedUpdate()
    {
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
    }
}
