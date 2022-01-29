using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private Image _healthSprite;
    [SerializeField] private PlayerMovement _playerMovement;
    private float _maxHealth = 100;
    private float _currentHealt = 100;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _playerMovement.BloodParticle();
            TakeDamage(20, gameObject);

            Destroy(other.gameObject);
                

        }
    }

    void TakeDamage(int damage, GameObject hitObject)
    {
        if (_currentHealt <= 0)
        {
            _playerMovement.BloodParticle();
            hitObject.transform.GetComponent<EnemyMovement>().enabled = false;

            if (hitObject != null)
            {
                Destroy(hitObject,.5f);

            }
        }

        _currentHealt -= damage;
        _healthSprite.fillAmount = _currentHealt / _maxHealth;
    }
}
