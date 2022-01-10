using System.Collections;
using System.Collections.Generic;

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
        }
    }
    void Start()
    {

    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1 || level == 3)
        {
           text = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            if(!text)
                Debug.LogError("Missing score text form scene");
        }
    }
    void EnemyDeath()
    {
        score += 10;
        text.text = "Score: " + score;
       
    }

    private void OnEnable()
    {
        GameController.OnEnemyDeath += EnemyDeath;
    }

    private void OnDisable()
    {
        GameController.OnEnemyDeath -= EnemyDeath;
        //We do this in order to avoid saving 'ghost' score system scores.
        if(hiscores.Length>0)
        {
            SaveLoad.SaveScores(hiscores);
        }
    }
}
