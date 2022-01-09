using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void ChangeDirection();
    public static event ChangeDirection OnSideCollision;

    public delegate void EnemyDeath();
    public static event EnemyDeath OnEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void EnemyDied()
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath();
    }

    public static void SideCollision()
    {
        if (OnSideCollision != null)
            OnSideCollision();
    }
}
