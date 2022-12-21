using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterParams")]
public class CharacterParams : ScriptableObject
{
    public float runSpeed = 10f;
    public bool canJump = true;
    public bool canRoll = true;
    public float skillForce = 10f;
}
