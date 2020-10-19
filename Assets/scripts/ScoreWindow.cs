using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI highscoreText;

    private void Awake()
    {
        //for(int i = 0; i < transform.childCount; i++)
        //{
        //    Debug.Log(i + ": " + transform.GetChild(i).name);
        //    foreach (Component component in transform.GetChild(i).GetComponents<Component>())
        //    {
        //        Debug.Log(component.GetType() + " , " + component.name); 
        //    }
        //    if (transform.GetChild(i).name.Equals("scoreText"))
        //        scoreText = transform.GetChild(i).GetComponent<Text>() ; 
        //}

        //scoreText = transform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>();
        //highscoreText = transform.Find("HighscoreText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
        highscoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();
    }

    private void Update()
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
    }
}
