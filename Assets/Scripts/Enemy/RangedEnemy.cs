using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RangedEnemy : Enemy
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


	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().GetHit(collisionAttackPower, transform.position);
		}
	}


	IEnumerator HitTime()
	{
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isInvincible = false;
	}


	override public IEnumerator DestroyEnemy()
	{
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}

    public override void Knockback(Vector3 hitPosition, float knockbackMultiplier)
    {
        
    }
}
