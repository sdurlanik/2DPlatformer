using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            Animator otherAnim = other.gameObject.GetComponent<Animator>();
            otherAnim.SetBool("isCollected", true);

        }
    }
    
}
