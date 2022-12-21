using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PeterSubController))]
public class PeterAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject projectilePrefab;
    
    private CharacterController2D mainController;
    private Animator animator;
    private Vector2 lastAttackInputPoint;


    void Start()
    {
        animator = GetComponent<Animator>();
        mainController = GetComponent<PeterSubController>().parentController;
    }

    void Update()
    {
        if (input.attackInput)
        {
            lastAttackInputPoint = input.attackPoint;
            animator.SetTrigger("Attack");
        }
    }

    public void OnAttackFrame()
    {
        Vector2 startPosition = attackPoint.position;
        GameObject newProjectile = Instantiate(projectilePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f));
        ThrowableWeapon projectileController = newProjectile.GetComponent<ThrowableWeapon>();
        
        projectileController.direction = (lastAttackInputPoint - startPosition).normalized;
        projectileController.StartMovement();
    }
}
