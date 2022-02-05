using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletExitPosition : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private PlayerMovement _playerMovement;
    public float angle;
    private Vector3 _pos;
    private Vector3 _dir;

    private bool _canShoot;
    private int _bananaCount;


    private void Start()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        _bananaCount = GameManager.Instance.BananaCount;

        _pos = Camera.main.WorldToScreenPoint(transform.position);
        _dir = Input.mousePosition - _pos;

        
        _playerAttack.CursorPos();


        if (!EventSystem.current.IsPointerOverGameObject())
        {
            CursorRange(_dir, _pos);
            if (CursorHide()) _playerAttack._cursorSprite.SetActive(false);

        }

        if (!Ability.instance.CanShoot)
        {
            _playerAttack._cursorSprite.SetActive(false);
        }

    }


    public  bool CursorHide()
    {
        if ((_playerMovement._facingRight && _dir.x <= -71) || (!_playerMovement._facingRight && _dir.x >= 71))
            
            return true;
        return false;
    }

    void CursorRange(Vector3 dir, Vector3 pos)
    {
        _playerAttack._cursorSprite.SetActive(true);

        if (((dir.x <= 90 && dir.x >= -90) || _playerMovement._onWall))
        {
            _playerAttack._cursorSprite.SetActive(false);
            _canShoot = false;
            return;
        }

        _canShoot = true;

        if (_canShoot && Ability.instance.CanShoot)
        {
            if (_bananaCount > 0)
            {
                _playerAttack.ShootBullet();
            }
            else
            {
                Ability.instance.CanShoot = false;

            }



        }

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
