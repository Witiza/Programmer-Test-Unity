using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    int score = 0;
    int[] hiscores;
    Text text;
    private void Awake()
    {
        SingletonCheck();
    }
     private void SingletonCheck()
    {
        //We check if this GO already exists (which will happen when we restart the game or go to the score screen.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            if(hiscores == null)
                hiscores = SaveLoad.LoadScores();
            SetupTexts(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        //Somehow, this function is called even before awake is called
        SingletonCheck();
    }
    void SetupTexts(int level)
    {
        if (level == 2  || level == 3)
        {
            text = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            if (!text)
            {
                Debug.LogError("Missing score text form scene");
            }
            else
            {
                if (level == 2)
                {
                    SetupHighscores();
                }
            }
        }
    }
   

    void SetupHighscores()
    {
        //Cant find a way to write this only one time without occluding the code.
        if(score>0)
        {
            CheckForHighscore();
        }

        string score_text = text.text;
        for (int i = 0;i<9;++i)
        {
            score_text += i + 1 + ": " + hiscores[i] +"\n";
        }

        if(score >0)
        {
            score_text += "\n" + "Last: " + score;
        }

        text.text = score_text;
        score = 0;
    }

    void CheckForHighscore()
    {
        for(int i = 0;i<9;++i)
        {
            if(hiscores[i]<score)
            {
                hiscores[i] = score;
                break;
            }
        }
        SaveLoad.SaveScores(hiscores);
    }

    void EnemyDeath()
    {
        score += 10;
        text.text = "Score: " + score;
    }

    void BulletMiss()
    {
        score -= 5;
        score = score < 0 ? 0 : score;
        text.text = "Score: " + score;
    }

    void ObstacleHit()
    {
        score -= 1;
        score = score < 0 ? 0 : score;
        text.text = "Score: " + score;
    }

    private void OnEnable()
    {
        GameController.OnEnemyDeath += EnemyDeath;
        GameController.OnMissedBullet += BulletMiss;
        GameController.OnObstacleHit += ObstacleHit;

    }

    private void OnDisable()
    {
        GameController.OnEnemyDeath -= EnemyDeath;
        GameController.OnMissedBullet -= BulletMiss;
        GameController.OnObstacleHit -= ObstacleHit;

    }
}
