using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerMaxHealth;
    private int _playerCurrentHealth;

    private void Start()
    {
        _playerCurrentHealth = _playerMaxHealth;
    }

    public void TakeDamage(int damage)
    {
        _playerCurrentHealth -= damage;
        print("CAN: " + _playerCurrentHealth);

        if (_playerCurrentHealth <= 0)
        {
            GameManager.Instance.ChangeState(GameManager.GameStates.DEATH);
        }
    }
}
