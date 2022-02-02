using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletExitPoint;
    [SerializeField] private float _bulletShootingForce;
    [SerializeField] private float _timer = 0f;
    [SerializeField] private float _waitingTime = 2f;
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

    }

    private void FixedUpdate()
    {
        PlayerFacetoMousePos();
    }

    public void ShootBullet()
    {
        _timer += Time.deltaTime;

        if (_timer > _waitingTime)
        {
            if (Input.GetMouseButtonDown(0) && !_playerMovementScript._onWall)
            {

                if (_bulletExitPosition.angle <= 70 && _bulletExitPosition.angle >= -70 && _playerMovementScript._facingRight)
                {
                    GameObject tempBullet = Instantiate(_bulletPrefab, _bulletExitPoint.transform.position,
                        _bulletExitPoint.transform.rotation);

                    Rigidbody2D tempRb = tempBullet.GetComponent<Rigidbody2D>();
                    tempRb.AddForce(_bulletExitPoint.transform.right * _bulletShootingForce, ForceMode2D.Force );

                    if (tempBullet != null)
                    {
                        StartCoroutine(DeactivateCollider(tempBullet));
                        StartCoroutine(DestroyObject(tempBullet));
                    }

                    ShakeCamera.ShakeCam.Shake(.5f,.1f);
                    EffectManager.Instance.PlayEffectSound(EffectManager.EffectState.ATTACK);
                    
                    GameManager.Instance.BananaCount--;
                    GameManager.Instance.BananaText.text = "X" + GameManager.Instance.BananaCount;
                }
                else if (((_bulletExitPosition.angle >= 110 && _bulletExitPosition.angle <=180) || (_bulletExitPosition.angle <= -110 && _bulletExitPosition.angle >= -180 )) && !_playerMovementScript._facingRight)
                {
                    GameObject tempBullet = Instantiate(_bulletPrefab, _bulletExitPoint.transform.position,
                        _bulletExitPoint.transform.rotation);

                    Rigidbody2D tempRb = tempBullet.GetComponent<Rigidbody2D>();
                    tempRb.AddForce(_bulletExitPoint.transform.right * _bulletShootingForce, ForceMode2D.Force );

                    if (tempBullet != null)
                    {
                        StartCoroutine(DeactivateCollider(tempBullet));
                        StartCoroutine(DestroyObject(tempBullet));
                    }
                    
                    ShakeCamera.ShakeCam.Shake(.5f,.1f);
                    EffectManager.Instance.PlayEffectSound(EffectManager.EffectState.ATTACK);
                    
                    GameManager.Instance.BananaCount--;
                    GameManager.Instance.BananaText.text = "X" + GameManager.Instance.BananaCount;
                }

                _timer = 0;
            
            }

            
        }
        
    }

    IEnumerator DestroyObject(GameObject willDestroy)
    {
        yield return new WaitForSeconds(5);
        
        if(willDestroy != null) Destroy(willDestroy);
    }

    IEnumerator DeactivateCollider(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);

        if (gameObject != null) gameObject.tag = "Empty";
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
        if (_bulletExitPosition.angle <= 80 && _bulletExitPosition.angle >= -80 && !_playerMovementScript._facingRight && !_playerMovementScript._onWall)
        {
            transform.Rotate(0f, 180f, 0f);
            _playerMovementScript._facingRight = true;  
        }
   
        else if (((_bulletExitPosition.angle >= 91 && _bulletExitPosition.angle <=180) || (_bulletExitPosition.angle <= -91 && _bulletExitPosition.angle >= -180 )) && _playerMovementScript._facingRight &&  !_playerMovementScript._onWall)

        {
            transform.Rotate(0f, 180f, 0f);
            _playerMovementScript._facingRight = false;
        }

       
    }
}
