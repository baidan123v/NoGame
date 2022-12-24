using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyProjectile : MonoBehaviour
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

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			GameObject player = col.gameObject;

			player.GetComponent<CharacterSubController>().parentController.contactDamageController.GetHit(dmgValue, transform.position);
			Destroy(gameObject);
		}
		else if (!col.gameObject.CompareTag("Enemy"))
		{
			Destroy(gameObject);
		}
	}
}
