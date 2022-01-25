using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{   [Header("Components")]
    private Rigidbody2D _rb;
    
    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 70f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 7f;
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);
    private bool _facingRight = true;
    private bool _canMove => _test;
    private bool _test = true;
    
    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;
    
    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _downMultiplier = 12f;
    [SerializeField] private int _extraJumps = 1;
    [SerializeField] private float _hangTime = .1f;
    [SerializeField] private float _jumpBufferLength = .1f;
    private int _extraJumpsValue;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0f && (_hangTimeCounter > 0f || _extraJumpsValue > 0 /*|| _onWall*/);
    private bool _isJumping;
    
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        if (Input.GetButtonDown("Jump")) _jumpBufferCounter = _jumpBufferLength;
        else _jumpBufferCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        if (_canMove) MoveCharacter();
        else
            // karakteri yavaşlatarak durdurur (Aniden durmasını engeller)

            _rb.velocity = Vector2.Lerp(_rb.velocity,
                (new Vector2(_horizontalDirection * _maxMoveSpeed, _rb.velocity.y)), .5f * Time.deltaTime);
        if (_onGround)
        {
            // Yerdeyse yer sürtünmesini etkinleştirir ve zıplama koşullarını ayarlar
            
            ApplyGroundLinearDrag();
            _extraJumpsValue = _extraJumps;
            _hangTimeCounter = _hangTime;
        }
        else
        {
            // Yerde değilse hava sürtünmesini etkinleştirir ve düşüş çarpanını ayarlar
            ApplyAirLinearDrag();
            FallMultiplier();
            
            // Havada kalma süresini ve zıplama koşulunu kontrol eder
            _hangTimeCounter -= Time.fixedDeltaTime;
            if (/*!_onWall ||*/ _rb.velocity.y < 0f /*|| _wallRun*/) _isJumping = false;
        }
        
        if (_canJump)
        {
            Jump(Vector2.up);
        }

    }

    private Vector2 GetInput()
    {
        // Klavye girdilerini alır WASD değerleri -1 ile 1 arasında Vector2 değer döndürür
        
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    
    // Karakter hareketi
    
    private void MoveCharacter()
    {
        // Karaktere belirtilen yön ve hızlanma girdisi ile güç uygular 
        
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);
        
        // Karakterin hızını maxMoveSpeed ile sınırlar
        
       if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }
    
    
    // Zemin sürtünmesini ayarlar
    
    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }
    
    // Hava sürtünmesini ayarlar
    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }
    
    // Düşüş çarpanını ayarlar (Yerçekimini havada olma koşuluna göre değiştir)
    private void FallMultiplier()
    {
        if (_verticalDirection < 0f)
        {
            _rb.gravityScale = _downMultiplier;
        }
        else
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rb.gravityScale = 1f;
            }
        }
    }
    
    
    // Collision kontrolleri
    
    private void CheckCollisions()
    {
        //Ground Collisions
        
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);

    }
    
    // Zıplama Kontrolü
    private void Jump(Vector2 direction)
    {
        if (!_onGround/* && !_onWall*/)
            _extraJumpsValue--;

        ApplyAirLinearDrag();
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
        _hangTimeCounter = 0f;
        _jumpBufferCounter = 0f;
        _isJumping = true;
    }
}
