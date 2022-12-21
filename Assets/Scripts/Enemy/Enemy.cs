using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

	public float life = 10;
	public float speed = 3f;
	public float collisionAttackPower = 15f;
	public EnemyState state {get; private set;} = EnemyState.Waiting;
	[SerializeField] protected bool isInvincible = false;


	void Update()
	{
		if (life <= 0) {
			Die();
		}
	}


	public void SetState(EnemyState newState)
    {
        state = newState;
    }


	public void GetHit(float damage, Vector3 hitPosition)
	{
		Debug.Log("Enemy hit");
		Debug.Log(gameObject);
		if (!isInvincible)
		{
			ApplyDamage(damage);
			Knockback(hitPosition, 600f);
		}
	}


	public void ApplyDamage(float damage)
	{
		life -= damage;
		StartCoroutine(HitTime());
	}


	public abstract void Knockback(Vector3 hitPosition, float knockbackMultiplier);


	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().GetHit(collisionAttackPower, transform.position);
		}
	}

	
	public void Die()
	{
		SetState(EnemyState.Dead);
		StartCoroutine(DestroyEnemy());
	}


	IEnumerator HitTime()
	{
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isInvincible = false;
	}


	public abstract IEnumerator DestroyEnemy();
}


public enum EnemyState {
    Waiting,
    Attacking,
	Dead
}
