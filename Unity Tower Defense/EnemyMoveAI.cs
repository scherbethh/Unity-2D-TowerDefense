using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMoveAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool lastEnemy;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject slowEffectObject; // Donma efekti referansý
    [SerializeField] private AudioClip deathSound;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int enemyHit = 0;
    [SerializeField] private float slowEffectMultiplier = 0.5f; // Yavaþlatma çarpaný
    private float currentSpeed;

    private Transform target;
    private GameController gameController;
    private AudioManager audioManager;

    [HideInInspector] public int pathIndex = 0;
    [HideInInspector] public static int currentHealth = 20;

    public static UnityEvent onEnemyDestroyed = new UnityEvent();
    public static UnityEvent onEnemySpawned = new UnityEvent();

    public static int enemyCount;

    private bool isDead = false;  // Düþmanýn ölü olup olmadýðýný takip eder

    public bool slowEffect = false; // Yavaþlatma etkisi olup olmadýðýný belirler

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        target = WaveManage.main.path[pathIndex];
        currentSpeed = moveSpeed;

        // Donma efektini baþlangýçta pasif yap
        if (slowEffectObject != null)
        {
            slowEffectObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.2f)
        {
            pathIndex++;

            if (pathIndex == WaveManage.main.path.Length)
            {
                PlayerHealth();
                gameController.OnEnemyKilled();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = WaveManage.main.path[pathIndex];
            }
        }

        // Donma efektini kontrol et
        if (slowEffect)
        {
            if (slowEffectObject != null && !slowEffectObject.activeInHierarchy)
            {
                slowEffectObject.SetActive(true);
            }
        }
        else
        {
            if (slowEffectObject != null && slowEffectObject.activeInHierarchy)
            {
                slowEffectObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (slowEffect)
        {
            currentSpeed = moveSpeed * slowEffectMultiplier;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * currentSpeed;
    }

    public int PlayerHealth()
    {
        currentHealth = currentHealth - enemyHit;
        return currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("bullet") && !isDead)
        {
            isDead = true;  // Düþmanýn öldüðünü iþaretle

            if (!lastEnemy)
            {
                Destroy(gameObject);
                DeadAnim();

                Instantiate(explosionEffect, transform.position, Quaternion.identity);  // Patlama efektini oluþtur
                GameObject prefabToSpawn = enemyPrefabs[0];
                GameObject newEnemy = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                EnemyMoveAI newEnemyAI = newEnemy.GetComponent<EnemyMoveAI>();
                newEnemyAI.pathIndex = this.pathIndex;
            }
            else
            {
                Destroy(gameObject);
                DeadAnim();
                Instantiate(explosionEffect, transform.position, Quaternion.identity);  // Patlama efektini oluþtur
                gameController.OnEnemyKilled();  // Düþman öldüðünde GameController'ý bilgilendir
            }
        }
    }

    public void DeadAnim()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);  // Patlama efektini oluþtur
        audioManager.PlaySFX(audioManager.death);
    }
}
