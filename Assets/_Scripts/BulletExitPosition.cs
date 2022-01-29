using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExitPosition : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;
    public float angle;
    private Vector3 _pos;
    private Vector3 _dir;

    private bool _canShoot;
    

    private void Start()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {   
        _pos = Camera.main.WorldToScreenPoint(transform.position);
         _dir= Input.mousePosition - _pos;

       CursorRange(_dir,_pos);

       if ((_playerMovement._facingRight && _dir.x <= -71) || (!_playerMovement._facingRight && _dir.x >= 71))
       {
           _playerAttack._cursorSprite.SetActive(false);
       }
       
      // print("Pozisyon " + _pos + "MousePoz " + Input.mousePosition + "Direction " + _dir);

        
    }

    void CursorRange(Vector3 dir, Vector3 pos)
    {
        _playerAttack._cursorSprite.SetActive(true);
     
        if ((dir.x <= 90 && dir.x >= -90) || _playerMovement._onWall)
        {
            _playerAttack._cursorSprite.SetActive(false);
            _canShoot = false;
            return;
        }
        
        

        _canShoot = true;
        if (_canShoot)
        {
            _playerAttack.ShootBullet();

        }
        
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
