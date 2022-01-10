using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    int score = 0;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}
