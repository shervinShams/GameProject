using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuContoller : MonoBehaviour
{

    public AudioSource backgroundMusic;
    public float quitingTime = 2.0f;
    public Image darkeningImage;

    private float timer = 0.0f;
    private bool needsQuit = false;

    public delegate void QuitAction();
    QuitAction onQuitAction;

    // Public methods

    public void StopGameToMainMenu()
    {
        BeginQuitingWithAction(LoadMainMenuScene);
    }
    

    public void StartSingleGame()
    {
        BeginQuitingWithAction(LoadSingGameScene);
    }

   
    public void StartLevelSnowGame()
    {
        BeginQuitingWithAction(LoadSnowScene);
    }

    public void StartLevelLavaGame()
    {
        BeginQuitingWithAction(LoadLavaScene);
    }

    public void StartLevelBossGame()
    {
        BeginQuitingWithAction(LoadBossScene);
    }



    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    //Private methods

    void FixedUpdate()
    {
        if (needsQuit)
        {
            timer += Time.deltaTime;
            if (backgroundMusic)
                backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, 0.0f, quitingTime * Time.deltaTime);

            if (darkeningImage)
                darkeningImage.color = Color.Lerp(darkeningImage.color, Color.black, quitingTime * Time.deltaTime);

            if (timer >= quitingTime)
            {
                onQuitAction();
            }
        }
    }

  public void BeginQuitingWithAction(QuitAction action)
    {
        onQuitAction = action;
        if (darkeningImage)
            darkeningImage.gameObject.SetActive(true);
        needsQuit = true;
    }

    void LoadSingGameScene()
    {
        SceneManager.LoadScene("SIngleGame");
    }

    void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

   void LoadSnowScene()
    {
        SceneManager.LoadScene("LevelSnow");
    }

    void LoadLavaScene()
    {
        SceneManager.LoadScene("LevelLava");
    }

    void LoadBossScene()
    {
        SceneManager.LoadScene("LevelBoss");
    }
}
