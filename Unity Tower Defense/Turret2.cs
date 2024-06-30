using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotatePoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject rangeIndicator;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float bps = 25f; // Saniyede olu�turulan "bullet" say�s�.
    [SerializeField] private float bulletSpeed = 3.0f;

    private Transform target;
    private float timeUntilFire;
    private bool isShooting = false;

    private bool isPlaced = false; // Turret'in yerle�tirilip yerle�tirilmedi�ini kontrol eder

    Animator turret2Animator;

    void Start()
    {
       
        turret2Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlaced)
        {
            
            if (target == null)
            {
                
                FindTarget();
            }
            else
            {
                
                RotateTowardsTarget();

                if (!CheckTargetIsInRange())
                {
                    target = null;
                }
                else
                {
                    timeUntilFire += Time.deltaTime;

                    if (timeUntilFire >= 1f / bps)
                    {
                        Shoot();
                        timeUntilFire = 0f;
                    }
                }
            }

            if (turret2Animator.GetBool("onShoot2") && !isShooting)
            {
                StartCoroutine(StopAnimationAfterDuration(0.1f)); // Animasyon s�resi manuel olarak belirlendi
            }
        }
    }
    private void OnMouseDown()
    {
        // Turret'e t�kland���nda menzil g�stergesini aktif hale getirin ve boyutunu ayarlay�n
        if (rangeIndicator != null)
        {
            rangeIndicator.SetActive(true);
            rangeIndicator.transform.localScale = new Vector3(targetingRange * 2f, targetingRange * 2f, 1f); // Menzil g�stergesinin boyutunu turret'in menziline g�re ayarlay�n
        }
    }
    private IEnumerator StopAnimationAfterDuration(float duration)
    {
        isShooting = true;
        yield return new WaitForSeconds(duration);
        turret2Animator.SetBool("onShoot2", false);
        isShooting = false;
    }

    private void Shoot()
    {
        int numberOfBullets = 3; // Ate�lenecek mermi say�s�
        float spreadAngle = 4.5f; // Her merminin sapma a��s�
        

        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);

            // Mermilerin farkl� a��larda sapmas� i�in bir sapma a��s� hesapla
            float angleOffset = (i - (numberOfBullets - 1) / 2f) * spreadAngle;

            // Mermiyi yay�lma pozisyonunda olu�tur ve rotasyonunu sapma a��s�yla ayarla
            Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + angleOffset));
            bullet.transform.rotation = bulletRotation;

            // Merminin y�n�n� firing point'e do�ru ayarla
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bullet.transform.up * bulletSpeed; // bulletSpeed de�i�kenini uygun bir h�z de�eriyle ayarlay�n
        }

        turret2Animator.SetBool("onShoot2", true);
        StartCoroutine(StopAnimationAfterDuration(0.1f)); // Animasyonun s�resi manuel olarak belirlendi
    }






    private void FindTarget()
    {
        Debug.Log("findtarget");
        float maxPathIndex = float.MinValue;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        foreach (var hit in hits)
        {
            EnemyMoveAI enemy = hit.transform.GetComponent<EnemyMoveAI>();
            if (enemy != null && enemy.pathIndex > maxPathIndex)
            {
                maxPathIndex = enemy.pathIndex;
                target = hit.transform;
            }
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    /*private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }*/

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }
}
