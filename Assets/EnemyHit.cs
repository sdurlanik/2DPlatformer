using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private Image _healthSprite;
    private float _maxHealth = 100;
    private float _currentHealt = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(20, gameObject);

        }
    }

    void TakeDamage(int damage, GameObject hitObject)
    {
        if (_currentHealt <= 0)
        {
            Destroy(hitObject);
        }

        _currentHealt -= damage;
        _healthSprite.fillAmount = _currentHealt / _maxHealth;
    }
}
