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
    private float _maxHealth = 90;
    private float _currentHealt = 90;
    
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
       

        _currentHealt -= damage;
        _healthSprite.fillAmount = _currentHealt / _maxHealth;
        
        if (_currentHealt <= 0)
        {
            _playerMovement.BloodParticle();
            hitObject.transform.GetComponent<EnemyMovement>().enabled = false;
            Destroy(hitObject, .5f);

        }
        
    }
}
