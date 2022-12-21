using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("here");
            Debug.Log(col.gameObject);
            col.gameObject.GetComponent<CharacterController2D>().ApplyDamage(99999);
        }
    }
}
