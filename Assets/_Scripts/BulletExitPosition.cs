using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExitPosition : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    public float angle;
    private Vector3 _pos;
    private Vector3 _dir;

    private bool _canShoot;
    

    private void Start()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    void Update()
    {
        _pos = Camera.main.WorldToScreenPoint(transform.position);
         _dir= Input.mousePosition - _pos;

       CursorRange(_dir,_pos);
      
      // print("Pozisyon " + _pos + "MousePoz " + Input.mousePosition + "Direction " + _dir);

        
    }

    void CursorRange(Vector3 dir, Vector3 pos)
    {
        _playerAttack._cursorSprite.SetActive(true);
     
        if (dir.x <= 90 && dir.x >= -90)
        {
            print("Update");
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
