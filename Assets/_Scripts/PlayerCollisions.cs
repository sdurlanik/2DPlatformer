using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerHealth PlayerHealthScript;
    private Transform _checkPoint;

    private void Start()
    {
        PlayerHealthScript = GetComponent<PlayerHealth>();
        _checkPoint = transform;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            Animator otherAnim = other.gameObject.GetComponent<Animator>();
            otherAnim.SetBool("isCollected", true);
            EffectManager.Instance.PlayEffectSound(EffectManager.EffectState.COLLECT);
        }

        if (other.gameObject.CompareTag("Trap"))
        {
            PlayerHealthScript.TakeDamage(1);
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            _checkPoint = other.transform;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("FallTrigger"))
        {
            transform.position = _checkPoint.position;
            PlayerHealthScript.TakeDamage(1);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerHealthScript.TakeDamage(1);
        }
    }

}
