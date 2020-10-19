using System;
using UnityEngine;

public static class Score
{

    public static void Start()
    {
        Bird.OnDied += BirdDeadHandler;
    }

    private static void BirdDeadHandler()
    {
        TrySetNewHighscore(Level.GetInstance().GetPipePassedCount());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("Highscore");
    }

    public static bool TrySetNewHighscore(int score)
    {
        Debug.Log("try set new highscore");
        if (score > GetHighScore())
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
            return false;
    }

    public static void ResetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.Save();
    }
}
