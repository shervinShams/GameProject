using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private bool needsQuit = false;
    public Image darkeningImage;
    public float quitingTime = 1.0f;
    private float restartTimer = 0.0f;
    public float restartDelay = 3.0f;

    //public PlayerPrefs playerPrefs;


    //public MainMenuContoller menuController;

    public delegate void QuitAction();
    QuitAction onQuitAction;

    public GameObject mainMenu;
    //public PlayerHealth playerHealth1;

    public GameObject player1;
    public GameObject player2;
    //public ScoreUpdater score;

    //public Text restartButtonText;
    //public GameObject resumeGameButton;
    public Text winText;

    public bool isGameBegan = true;
    public bool isPaused = false;
    public bool isMenuShown = false;

    public float HeartAppearRate = 30.0f;
    public float NuclearAppearRate = 7.0f;

    private int Player1Score = 0;
    private int Player2Score = 0;
    private float timer = 0.0f;

    private BoardManager boardScript;
    private float heartTimer = 0.0f;
    private float nuclearTimer = 0.0f;

    void Awake()
    {
        //playerPrefs = GetComponent<PlayerPrefs>();
        //playerHealth1 = GetComponent<PlayerHealth>();
        boardScript = GetComponent<BoardManager>();
        winText.enabled = false;
        StartGame();
        PlayerPrefs.DeleteKey("Score1");
        PlayerPrefs.DeleteKey("Score2");
        //DontDestroyOnLoad(playerHealth1);
        // DontDestroyOnLoad(playerPrefs);
        //DontDestroyOnLoad(Player1Score);
        //playerPrefs = GetComponent<PlayerPrefs>();
    }

  

    void Update()
    {
        bool escape = Input.GetButtonDown("Cancel");
        if (escape)
        {
            if (isGameBegan)
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }

        bool enter = Input.GetButtonDown("Submit");
        if (enter)
        {
            if (!isGameBegan && !isMenuShown)
            {
                StartGame();
            }
        }

        restartTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isGameBegan || !isPaused)
        {
            heartTimer += Time.deltaTime;
            nuclearTimer += Time.deltaTime;

            if (heartTimer >= HeartAppearRate)
            {
                boardScript.AddRandomHeart();
                heartTimer = 0.0f;
            }

            if (nuclearTimer >= NuclearAppearRate)
            {
                boardScript.AddRandomNuclearPickup();
                nuclearTimer = 0.0f;
            }
        }
        //timer += Time.deltaTime;
       // darkeningImage.color = Color.Lerp(darkeningImage.color, Color.black, quitingTime * Time.deltaTime);

        if (needsQuit)
        {
            timer += Time.deltaTime;
            //if (backgroundMusic)
            //    backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, 0.0f, quitingTime * Time.deltaTime);

            if (darkeningImage)
                darkeningImage.color = Color.Lerp(darkeningImage.color, Color.black, quitingTime * Time.deltaTime);

            if (timer >= quitingTime)
            {
                onQuitAction();
            }
        }

    }

    void ResetPlayers()
    {
        PlayerController player1Controller = player1.GetComponentInChildren<PlayerController>();
        player1Controller.ResetPosition();
        PlayerHealth player1Health = player1.GetComponentInChildren<PlayerHealth>();
        player1Health.ResetHealth();

        PlayerController player2Controller = player2.GetComponentInChildren<PlayerController>();
        player2Controller.ResetPosition();
        PlayerHealth player2Health = player2.GetComponentInChildren<PlayerHealth>();
        player2Health.ResetHealth();
    }

    public static bool isGamePaused()
    {
       
        
      GameObject gManager = GameObject.Find("GameManager");
      return gManager.GetComponent<GameManager>().isPaused;
     
       
    }

    public void StartGame()
    {
        winText.text = "";
        winText.enabled = false;
        boardScript.SetupScene();
        ResetPlayers();
        isGameBegan = true;
        ResumeGame();
        isPaused = false;
    }

    public void PauseGame()
    {
        mainMenu.SetActive(true);
        isPaused = true;
        isMenuShown = true;
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        isPaused = false;
        isMenuShown = false;
    }

    public void FinishGame(bool firstPlayerWin)
    {
        isGameBegan = false;
        isPaused = true;

        string playerNum;

        if (!firstPlayerWin)
        {
            
            playerNum = "2";
            Player2Score++;
            PlayerPrefs.SetInt("Score2", Player2Score);
            
        }

        else
        {
            playerNum = "1";
            Player1Score++;
            PlayerPrefs.SetInt("Score1", Player1Score);
            
        }

        winText.text = "Player " + playerNum + " Wins!!!!!\nScore " + PlayerPrefs.GetInt("Score1") + " : " + PlayerPrefs.GetInt("Score2") + "\nPress enter to\ncontinue";
        winText.enabled = true;

        //if (isPaused && !isGameBegan)
        //{
        //    Invoke("LoadSnowScene", 5);
        //    transform.position += new Vector3(0, 1, 2);
        //    Invoke("delayedRotation", 4f);
        //    //ResumeGame();
        //    GetScoreForPlayer(1);

        //}




        // .. if it reaches the restart delay...
        if (restartTimer >= restartDelay)
        {
            BeginQuitingWithAction(LoadSnowScene);
        }



        // Invoke("BeginQuitingWithAction", 5);

    }

    void BeginQuitingWithAction(QuitAction action)
    {
        onQuitAction = action;
        if (darkeningImage)
            darkeningImage.gameObject.SetActive(true);
        needsQuit = true;
        //LoadSnowScene();
    }

    void delayedRotation()
    {
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime);
        darkeningImage.gameObject.SetActive(true);
    }


    public int GetScoreForPlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            return Player1Score;
            
          
        }
        if (playerNum == 2)
            return Player2Score;

        return 0;
    }

    void LoadSnowScene()
    {
        SceneManager.LoadScene("LevelSnow");
    }

}
