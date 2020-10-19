using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI highscoreText;

    private void Awake()
    {

        // GameObject.Find will work too, but GameObject.Find will find from the root
        //scoreText = GameObject.Find("score").GetComponent<TMPro.TextMeshProUGUI>();
        //highscoreText = GameObject.Find("HighscoreText").GetComponent<TMPro.TextMeshProUGUI>();

        // Debug.Log(scoreText.text);
        Button retryButton = transform.Find("VerticalList/RetryButton").GetComponent<Button>();
        retryButton.onClick.AddListener(RetryHandler);
        retryButton.AddButtonSound();
        Button mainMenuButton = transform.Find("VerticalList/MainMenuButton").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(MainMenuHandler);
        mainMenuButton.AddButtonSound();

        Bird.OnDied += BirdDeadHandler;
        Hide();
    }


    private void MainMenuHandler()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }
    private void RetryHandler()
    {

        Loader.Load(Loader.Scene.GameScene);
    }

    private void Start()
    {
        highscoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();
    }

    private void BirdDeadHandler()
    {
        Debug.Log("show gameover window");
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();

        Score.TrySetNewHighscore(Level.GetInstance().GetPipePassedCount());
        highscoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();
        highscoreText.gameObject.GetComponent<UAP_BaseElement>()?.SetCustomText("high score is " + Score.GetHighScore().ToString());

        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    /**
     * need to unsubscribe from event 
     * because subscribe basically mean tell the event trigger to notice everything that subscribe to the event 
     * as the event is static, the list of receiver will not be deleted 
     * and when the bird cannot find the old game over window, error occur
     */
    private void OnDestroy()
    {
        Bird.OnDied -= BirdDeadHandler;
        Destroy(gameObject);
    }

}
