using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerLava : MonoBehaviour {

    public GameObject mainMenu;
    public Image darkeningImage;
    public float quitingTime = 1.0f;
    private float restartTimer = 0f;
    public float restartDelay = 3.0f;

    public GameObject player1;
    public GameObject player2;

    //public Text restartButtonText;
    //public GameObject resumeGameButton;
    public Text winText;
    public Text gameOverTxt;

    public bool isGameBegan = true;
    public bool isPaused = false;
    public bool isMenuShown = false;

    public float HeartAppearRate = 30.0f;
    public float NuclearAppearRate = 7.0f;

    private int Player1Score = 0;
    private int Player2Score = 0;

    private BoardManager boardScript;
    private float heartTimer = 0.0f;
    private float nuclearTimer = 0.0f;
    private bool needsQuit = false;
    private float timer = 0.0f;


    public delegate void QuitAction();
    QuitAction onQuitAction;

    // private PlayerPrefs score1;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        winText.enabled = false;
        gameOverTxt.enabled = false;
        StartGame();
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
        PlayerControllerLava player1Controller = player1.GetComponentInChildren<PlayerControllerLava>();
        player1Controller.ResetPosition();
        PlayerHealth player1Health = player1.GetComponentInChildren<PlayerHealth>();
        player1Health.ResetHealth();

        PlayerControllerLava player2Controller = player2.GetComponentInChildren<PlayerControllerLava>();
        player2Controller.ResetPosition();
        PlayerHealth player2Health = player2.GetComponentInChildren<PlayerHealth>();
        player2Health.ResetHealth();
    }

    public static bool isGamePaused()
    {
        GameObject gManager = GameObject.Find("GameManagerLava");
        return gManager.GetComponent<GameManagerLava>().isPaused;
    }

    public void StartGame()
    {
        winText.text = "";
        winText.enabled = false;

        gameOverTxt.text = "";
        gameOverTxt.enabled = false;

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

            Player2Score = PlayerPrefs.GetInt("Score2");
            Player2Score++;
            PlayerPrefs.SetInt("Score2", Player2Score);
            GetHighScore(2);


        }
        else
        {
            playerNum = "1";

            Player1Score = PlayerPrefs.GetInt("Score1");
            Player1Score++;
            PlayerPrefs.SetInt("Score1", Player1Score);
            GetHighScore(1);
        }

        //gameOverTxt.text = "PLAYER " + playerNum + "IS THE WINNER !!!!!!";
        //gameOverTxt.enabled = true;

        //winText.text = "Player " + playerNum + " Wins!!!!!\nScore " + PlayerPrefs.GetInt("Score1") + " : " + PlayerPrefs.GetInt("Score2") + "\nPress enter to\ncontinue";
        //winText.enabled = true;



        if (restartTimer >= restartDelay)
        {
            BeginQuitingWithAction(LoadBossScene);
        }

    }


    void BeginQuitingWithAction(QuitAction action)
    {
        onQuitAction = action;
        if (darkeningImage)
            darkeningImage.gameObject.SetActive(true);
        needsQuit = true;
        //LoadSnowScene();
    }



    public int GetScoreForPlayer(int playerNum)
    {
        if (playerNum == 1)
            return Player1Score;
        if (playerNum == 2)
            return Player2Score;

        return 0;
    }

    public void GetHighScore(int playerNum)
    {

        int score1 = PlayerPrefs.GetInt("Score1");
        int score2 = PlayerPrefs.GetInt("Score2");

        if (score1 > score2 && score1 >= 3)
        {

            gameOverTxt.text = "PLAYER " + playerNum + "  \nwill Battle with the \nBOSS!!!";
            //winText.enabled = false;

            // winText.text = gameOverTxt.text;
            gameOverTxt.enabled = true;

            winText.enabled = false;
        }

        else if (score2 > score1 && score2 >= 3)
        {


            Debug.Log(score1);
            Debug.Log(score2);
            gameOverTxt.text = "PLAYER " + playerNum + "  \nwill Battle with the \nBOSS!!!";
            //winText.enabled = false;

            // winText.text = gameOverTxt.text;
            gameOverTxt.enabled = true;

            winText.enabled = false;
        }



        else
        {
            winText.text = "Player " + playerNum + " Wins!!!!!\nScore " + PlayerPrefs.GetInt("Score1") + " : " + PlayerPrefs.GetInt("Score2") + "\nPress enter to\ncontinue";
            winText.enabled = true;
            //gameOverTxt.enabled = true;

        }
    }


    void LoadBossScene()
    {
        SceneManager.LoadScene("LevelBoss");
    }

}