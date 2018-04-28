using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBoss : MonoBehaviour
{

    public GameObject mainMenu;
    private bool needsQuit = false;
    public Image darkeningImage;
    public float quitingTime = 5.0f;
 

    // public MainMenuContoller menuController;

    public bool winner = true;

    public GameObject player1;
    public GameObject player2;

    //public Text restartButtonText;
    //public GameObject resumeGameButton;
    public Text winText;
    public Text countDown;

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
    private float timer = 0.0f;



    public delegate void QuitAction();
    QuitAction onQuitAction;


    // private PlayerPrefs score1;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        winText.enabled = false;
        countDown.enabled = false;
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


    }

    public static bool isGamePaused()
    {
        GameObject gManager = GameObject.Find("GameManagerBoss");
        return gManager.GetComponent<GameManagerBoss>().isPaused;
    }

    public void StartGame()
    {
        winText.text = "";
        winText.enabled = false;

        countDown.text = "";
        countDown.enabled = false;

        boardScript.SetupScene();
        StartGameWithBoss(winner);
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

        GameManagerBoss gManager = GetComponent<GameManagerBoss>();
        gManager.isPaused = true;

        MyWinText(playerNum);

        StartCoroutine(UiHandler());
    }


    public void StartGameWithBoss(bool winner)

    {
        //isGameBegan = false;
        //isPaused = true;

      
        int score1 = PlayerPrefs.GetInt("Score1");
        int score2 = PlayerPrefs.GetInt("Score2");

        try
        {


            if (score1 > score2)
            {
                //Setup player1 as player1

                winner = false;
                BossController bossControler1 = player1.GetComponentInChildren<BossController>();
                bossControler1.enabled = !bossControler1.enabled;
                PlayerControllerBoss player1Controller = player1.GetComponentInChildren<PlayerControllerBoss>();
                player1Controller.ResetPosition();
                PlayerHealth player1Health = player1.GetComponentInChildren<PlayerHealth>();
                player1Health.ResetHealth();


                // Setup player2 as boss

                PlayerControllerBoss player2Controller = player2.GetComponentInChildren<PlayerControllerBoss>();
                player2Controller.ResetPosition();
                player2Controller.enabled = !player2Controller.enabled;
                BossController bossController = player2.GetComponentInChildren<BossController>();
                bossController.ResetPosition();
                PlayerHealth player2Health = player2.GetComponentInChildren<PlayerHealth>();
                player2Health.ResetHealth();
                Transform bossTransform2 = player2.GetComponentInChildren<Transform>();
                //bossTransform2.localScale += new Vector3(0.2F, 0.2f, 0);
                

                // PlayerPrefs.SetInt("Score2", Player2Score);
            }

            else if (score2 > score1)
            {
                // Setup player2 as player2

                winner = false;
                BossController bossControler2 = player2.GetComponentInChildren<BossController>();
                bossControler2.enabled = !bossControler2.enabled;
                PlayerControllerBoss player2Controller = player2.GetComponentInChildren<PlayerControllerBoss>();
                player2Controller.ResetPosition();
                PlayerHealth player2Health = player2.GetComponentInChildren<PlayerHealth>();
                player2Health.ResetHealth();

                //   setup player1 as boss

                PlayerControllerBoss player1Controller = player1.GetComponentInChildren<PlayerControllerBoss>();
                player1Controller.ResetPosition();
                player1Controller.enabled = !player1Controller.enabled;
                BossController bossController = player1.GetComponentInChildren<BossController>();
                bossController.ResetPosition();
                PlayerHealth player1Health = player1.GetComponentInChildren<PlayerHealth>();
                player1Health.ResetHealth();
            }

        }
        catch (NullReferenceException e)
        {


            Debug.Log(e);
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


    public void GetHighScore(int playerNum)
    {

        int score1 = PlayerPrefs.GetInt("Score1");
        int score2 = PlayerPrefs.GetInt("Score2");

    }

    public IEnumerator UiHandler()
    {

        yield return new WaitForSeconds(3.5f);
        winText.enabled = false;
        countDown.text = "First Level\n 3";
        countDown.enabled = true;
        yield return new WaitForSeconds(1);
        countDown.text = "First Level\n 2";
        countDown.enabled = true;
        yield return new WaitForSeconds(1);
        countDown.text = "First Level\n 1";
        countDown.enabled = true;
        yield return new WaitForSeconds(1);
        countDown.text = "Go";
        countDown.enabled = true;
        yield return new WaitForSeconds(0.2f);
        BeginQuitingWithAction(LoadLevelFirstScene);

    }

    public void MyWinText(string playerNumber)
    {
        isPaused = true;
        winText.text = "WINNER\nPLAYER " + playerNumber + " WIN\nScore " + PlayerPrefs.GetInt("Score1") + " : " + PlayerPrefs.GetInt("Score2");
        winText.enabled = true;

    }

    void LoadLevelFirstScene()
    {
        SceneManager.LoadScene("SIngleGame");
    }

}