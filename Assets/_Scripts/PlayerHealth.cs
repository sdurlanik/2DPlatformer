using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerMaxHealth;
    private int _playerCurrentHealth;

    [SerializeField] private Image[] _hearts;

    private void Start()
    {
        _playerCurrentHealth = _playerMaxHealth;
        UpdateHeart();
    }
    

    private void UpdateHeart()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < _playerCurrentHealth)
            {
                _hearts[i].color = Color.red;
            }
            else
            {
                _hearts[i].color = Color.white;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        _playerCurrentHealth -= damage;
UpdateHeart();
        StartCoroutine(HitAnim());

        if (_playerCurrentHealth <= 0)
        {
            GameManager.Instance.ChangeState(GameManager.GameStates.DEATH);
        }
    }

    IEnumerator HitAnim()
    {
        GetComponent<Animator>().SetBool("isHit", true);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetBool("isHit", false);
 }
}
