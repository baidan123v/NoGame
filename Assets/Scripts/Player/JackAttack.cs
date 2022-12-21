using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class JackAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerInput input;

    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (input.attackInput)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void PerformAttack()
    {
        Debug.Log("Attacked");
    }
}
