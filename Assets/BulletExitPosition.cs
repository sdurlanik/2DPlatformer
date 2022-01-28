using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExitPosition : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    public float angle;

    private void Start()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    void Update()
    {
        Vector3 offset = new Vector3(1, 1, 0);
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;

       deneme(dir,pos);
      
       print("Pozisyon " + pos + "MousePoz " + Input.mousePosition + "Direction " + dir);

        
    }

    void deneme(Vector3 dir, Vector3 pos)
    {
        if (dir.x <= 90 && dir.x >= -90)
        {
            print("Update");
            _playerAttack._cursorSprite.SetActive(false);
            return;
        }
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
