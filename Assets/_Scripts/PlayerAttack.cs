using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletExitPoint;
    [SerializeField] private float _bulletShootingForce;
    
    private PlayerMovement _playerMovementScript;
    private BulletExitPosition _bulletExitPosition;
    void Start()
    {
        _playerMovementScript = GetComponentInParent<PlayerMovement>();
        _bulletExitPosition = GetComponentInChildren<BulletExitPosition>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (_bulletExitPosition.angle <= 70 && _bulletExitPosition.angle >= -70 && _playerMovementScript._facingRight)
            {
                GameObject tempBullet = Instantiate(_bulletPrefab, _bulletExitPoint.transform.position,
                    _bulletExitPoint.transform.rotation);

                Rigidbody2D tempRb = tempBullet.GetComponent<Rigidbody2D>();
            
                tempRb.AddForce(_bulletExitPoint.transform.right * _bulletShootingForce, ForceMode2D.Force );
                Destroy(tempBullet, 5f);
            }
            else if (((_bulletExitPosition.angle >= 110 && _bulletExitPosition.angle <=180) || (_bulletExitPosition.angle <= -110 && _bulletExitPosition.angle >= -180 )) && !_playerMovementScript._facingRight)
            {
                GameObject tempBullet = Instantiate(_bulletPrefab, _bulletExitPoint.transform.position,
                    _bulletExitPoint.transform.rotation);

                Rigidbody2D tempRb = tempBullet.GetComponent<Rigidbody2D>();
            
                tempRb.AddForce(_bulletExitPoint.transform.right * _bulletShootingForce, ForceMode2D.Force );
                Destroy(tempBullet, 5f);
            }
            
        }
    }
}
