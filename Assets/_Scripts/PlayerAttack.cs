using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletExitPoint;
    [SerializeField] private float _bulletShootingForce;
    public GameObject _cursorSprite;
    
    private PlayerMovement _playerMovementScript;
    private BulletExitPosition _bulletExitPosition;
    void Start()
    {
        _playerMovementScript = GetComponentInParent<PlayerMovement>();
        _bulletExitPosition = GetComponentInChildren<BulletExitPosition>();
    }


    private void Update()
    {

        CursorPos();
        ShootBullet();

    }

    private void FixedUpdate()
    {
        PlayerFacetoMousePos();

    }

    void ShootBullet()
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

    void CursorPos()
    {
        if (_bulletExitPosition.angle <= 70 && _bulletExitPosition.angle >= -70 && _playerMovementScript._facingRight)
            _cursorSprite.SetActive(true);
        else if (((_bulletExitPosition.angle >= 110 && _bulletExitPosition.angle <=180) || (_bulletExitPosition.angle <= -110 && _bulletExitPosition.angle >= -180 )) && !_playerMovementScript._facingRight)
            _cursorSprite.SetActive(true);
        else _cursorSprite.SetActive(false);
    }
    void PlayerFacetoMousePos()
    {
        if (_bulletExitPosition.angle <= 80 && _bulletExitPosition.angle >= -80 && !_playerMovementScript._facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            _playerMovementScript._facingRight = true;  
        }
   
        else if (((_bulletExitPosition.angle >= 91 && _bulletExitPosition.angle <=180) || (_bulletExitPosition.angle <= -91 && _bulletExitPosition.angle >= -180 )) && _playerMovementScript._facingRight)

        {
            print("Girdi");
            transform.Rotate(0f, 180f, 0f);
            _playerMovementScript._facingRight = false;
        }

        else
        {
            


        }
       
    }
}