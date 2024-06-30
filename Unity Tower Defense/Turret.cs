using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotatePoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float bps = 25f; // Saniyede oluþturulan "bullet" sayýsý.

    private Transform target;
    private float timeUntilFire;
    private bool isShooting = false;

    private bool isPlaced = false; // Turret'in yerleþtirilip yerleþtirilmediðini kontrol eder

    Animator turretAnimator;

    void Start()
    {
        turretAnimator = GetComponent<Animator>();
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

            if (turretAnimator.GetBool("onShoot") && !isShooting)
            {
                StartCoroutine(StopAnimationAfterDuration(turretAnimator.GetCurrentAnimatorStateInfo(0).length));
            }
        }
    }

    private IEnumerator StopAnimationAfterDuration(float duration)
    {
        isShooting = true;
        yield return new WaitForSeconds(duration);
        turretAnimator.SetBool("onShoot", false);
        isShooting = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.transform.rotation = transform.rotation;
        bulletScript.SetTarget(target);

        turretAnimator.SetBool("onShoot", true);
    }

    private void FindTarget()
    {
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
