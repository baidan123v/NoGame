using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HPController))]
public class ContactDamageController : MonoBehaviour
{
    [SerializeField] private float knockbackMultiplier;

    private Rigidbody2D rb2d;
    public UnityEvent HitEvent;
    private HPController hpController;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hpController = GetComponent<HPController>();
    }


    public void GetHit(float damage, Vector3 hitPosition)
	{
		if (!hpController.isInvincible)
		{
			ApplyDamage(damage);
			Knockback(hitPosition);
            HitEvent?.Invoke();
		}
	}


	public void ApplyDamage(float damage) 
	{
		if (hpController.isInvincible)
		{
			return;
		}

        hpController.ChangeLifeByAmount(-damage);
	}


    public void Knockback(Vector3 hitPosition)
	{
		Vector2 damageDir = Vector3.Normalize(transform.position - hitPosition);
		damageDir.y = 0.4f;
		
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce(damageDir * knockbackMultiplier);
	}
}
