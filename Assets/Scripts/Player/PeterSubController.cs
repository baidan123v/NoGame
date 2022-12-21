using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PeterAttack))]
public class PeterSubController : CharacterSubController
{
    public PlayerMovement movement;

    public void RollEnd()
    {
        movement.OnRollEnd();
    }
}
