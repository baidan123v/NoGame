using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;



public class HPController : MonoBehaviour
{
    [SerializeField] public float maxLife = 10f;
    [SerializeField] public bool isInvincible = false;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float hitInvincibilityDurationSeconds = 1f;

    public float life {get; private set;}
    public UnityEvent DeathEvent;

    void Awake()
    {
        SetLife(maxLife);
    }


    public void ChangeLifeByAmount(float amount)
	{
		float newLife = life + amount;
		if (newLife > maxLife)
		{
			newLife = maxLife;
		}

        if (amount < 0)
        {
            StartCoroutine(MakeInvincible(hitInvincibilityDurationSeconds));
        }

		SetLife(newLife);
	}


    public void SetLife(float newLife)
	{
        if (newLife <= 0 && life > newLife)
        {
            DeathEvent?.Invoke();
            isInvincible = true;
        }

		life = newLife;
		healthBar?.SetHealth(newLife, maxLife);
	}


    IEnumerator MakeInvincible(float time) 
	{
		isInvincible = true;
		yield return new WaitForSeconds(time);
		isInvincible = false;
	}
}
