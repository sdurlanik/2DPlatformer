using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private GameObject _enemyUi;
    private Image _healthSprite;
    [SerializeField] private PlayerMovement _playerMovement;
    private float _maxHealth = 90;
    private float _currentHealt = 90;

    private void Start()
    {
        _healthSprite = _enemyUi.transform.GetChild(0).GetChild(1).GetComponent<Image>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _enemyUi.SetActive(true);
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
            _enemyUi.SetActive(false);
            Destroy(hitObject, .5f);

        }
        
    }
}
