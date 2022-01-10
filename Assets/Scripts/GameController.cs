using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public delegate void ChangeDirection();
    public static event ChangeDirection OnSideCollision;

    public delegate void EnemyDeath();
    public static event EnemyDeath OnEnemyDeath;

    public delegate void MissedBullet();
    public static event MissedBullet OnMissedBullet;

    public delegate void PlayerHit();
    public static event PlayerHit OnPlayerHit;

    public delegate void GameFinished();
    public static event GameFinished OnGameFinish;

    static int enemy_amount;
    static int player_lives;
    public GameObject game_over;
    public GameObject congratulations;
    bool finishing = false;





    void Start()
    {
        enemy_amount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        player_lives = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().player_lives;
    }

    private void Update()
    {
        if(!finishing && (enemy_amount <= 0||player_lives <0))
        {
            finishing = true;
            FinishedGame();
            StartCoroutine(EndGame());
        }
    }
    public static void BulletMiss()
    {
        OnMissedBullet?.Invoke();
    }
    public  static void EnemyDied()
    {
        OnEnemyDeath?.Invoke();
        enemy_amount--;
        if(enemy_amount<=0)
        {
            FinishedGame();
        }
    }

    public static void SideCollision()
    {
        OnSideCollision?.Invoke();
    }

    public static void HitPlayer()
    {
        OnPlayerHit?.Invoke();
        player_lives--;
    }

    public static void FinishedGame()
    {
        OnGameFinish?.Invoke();
    }

    IEnumerator EndGame()
    {
        if(enemy_amount == 0)
        {
            congratulations.SetActive(true);
        }
        else
        {
            game_over.SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Scores");
    }

}
