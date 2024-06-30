using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    private int enemyLayer;

    private void Start()
    {
        // Düþman layer'ýný belirleyin
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            EnemyMoveAI enemy = other.GetComponent<EnemyMoveAI>();
            if (enemy != null)
            {
                enemy.slowEffect = true;
                Debug.Log("Enemy slowed down in slow area");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            EnemyMoveAI enemy = other.GetComponent<EnemyMoveAI>();
            if (enemy != null)
            {
                enemy.slowEffect = false;
                Debug.Log("Enemy speed restored after leaving slow area");
            }
        }
    }
}
