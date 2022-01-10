using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    int score = 0;
    int[] hiscores = new int[10];
    Text text;
    private void Awake()
    {
        //We check if this GO already exists (which will happen when we restart the game or go to the score screen.
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            hiscores = SaveLoad.LoadScores();
            SetupTexts(SceneManager.GetActiveScene().buildIndex);

        }
    }
    void Start()
    {

    }

    private void OnLevelWasLoaded(int level)
    {
        SetupTexts(level);
    }
    void SetupTexts(int level)
    {

        if (level == 5 || level == 1 || level == 3)
        {
            text = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            if (!text)
            {
                Debug.LogError("Missing score text form scene");
            }
            else
            {
                Debug.Log("SCORE FOUND");
                if (level == 3)
                {
                    SetupHighscores();
                }
            }
        }
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
    }

    private void OnEnable()
    {
        GameController.OnEnemyDeath += EnemyDeath;
        GameController.OnMissedBullet += BulletMiss;
    }

    private void OnDisable()
    {
        GameController.OnEnemyDeath -= EnemyDeath;
        GameController.OnMissedBullet -= BulletMiss;

        //We do this in order to avoid saving 'ghost' score system scores.
        if (hiscores.Length>0)
        {
            SaveLoad.SaveScores(hiscores);
        }
    }
}
