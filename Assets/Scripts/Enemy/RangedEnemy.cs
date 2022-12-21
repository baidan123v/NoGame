using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private CircleCollider2D attackRange;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private float reloadSeconds = 1f;
    [SerializeField] private GameObject projectilePrefab;

    private bool canShoot = true;

    void OnTriggerStay2D(Collider2D col)
    {
        if (canShoot && col.gameObject.CompareTag("Player"))
        {
            canShoot = false;
            StartCoroutine(ResetShootAvailability());
            Shoot(col.gameObject.transform.position);
        }
    }


    private IEnumerator ResetShootAvailability()
    {
        yield return new WaitForSeconds(reloadSeconds);
        canShoot = true;
    }


    private void Shoot(Vector2 playerPosition)
    {  
        Vector2 startPosition = attackPosition.position;
        Vector2 direction = (playerPosition - startPosition).normalized;

        GameObject newProjectile = Instantiate(projectilePrefab, startPosition, Quaternion.Euler(0, 0, 0));
        EnemyProjectile projectileController = newProjectile.GetComponent<EnemyProjectile>();
        
        projectileController.direction = direction.normalized;
        projectileController.StartMovement();
    }
}
