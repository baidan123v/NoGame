using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float xMovement {get; private set;}
    public bool jumpInput {get; private set;}
    public bool meleeAttackInput {get; private set;}
    public bool rangeAttackInput {get; private set;}
    public bool switchCharacterInput {get; private set;}
    public bool attackInput {get; private set;}
    public Vector2 attackPoint {get; private set;}
    
    private KeyCode moveLeftKey = KeyCode.A;
    private KeyCode moveRightKey = KeyCode.D;
    private KeyCode jumpKey = KeyCode.W;
    private KeyCode jumpKey2 = KeyCode.Space;
    private KeyCode switchCharacterKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKey(moveRightKey) && !Input.GetKey(moveLeftKey))
        {
            xMovement = 1;
        }
        else if (Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
        {
            xMovement = -1;
        }
        else
        {
            xMovement = 0;
        }

        jumpInput = Input.GetKeyDown(jumpKey) || Input.GetKeyDown(jumpKey2);
        switchCharacterInput = Input.GetKeyDown(switchCharacterKey);
        attackInput = Input.GetMouseButtonDown(0);
        if (attackInput)
        {
            attackPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
