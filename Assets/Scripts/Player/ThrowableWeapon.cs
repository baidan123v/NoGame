using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ThrowableWeapon : MonoBehaviour
{
	public Vector2 direction;
	public bool hasHit = false;
	public float speed = 10f;
	public float dmgValue = 2f;

    public void StartMovement()
    {
		GetComponent<Rigidbody2D>().velocity = direction * speed;
		if (direction.x < 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.isTrigger && collision.gameObject.CompareTag("Enemy"))
		{
			GameObject enemy = collision.gameObject;

			enemy.GetComponent<Enemy>().GetHit(dmgValue, transform.position);
			Destroy(gameObject);
		}
		else if (!collision.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
