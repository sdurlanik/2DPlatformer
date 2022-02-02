using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Components")] private Rigidbody2D _rb;
    private Animator _anim;
    [SerializeField] private ParticleSystem _dustParticle;
    [SerializeField] private ParticleSystem _bloodParticle;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;


    [Header("Movement Variables")] [SerializeField]
    private float _movementAcceleration = 70f;

    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 7f;
    private float _horizontalDirection;
    private float _verticalDirection;

    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) ||
                                       (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    public bool _facingRight = true;

    private bool _canMove => !_wallGrab;

    [Header("Ground Collision Variables")] [SerializeField]
    private float _groundRaycastLength;

    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;

    [Header("Wall Collision Variables")] [SerializeField]
    private float _wallRaycastLength;

    public bool _onWall;
    private bool _onRightWall;

    [Header("Layer Masks")] [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField] private LayerMask _wallLayer;


    [Header("Jump Variables")] [SerializeField]
    private float _jumpForce = 12f;

    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _downMultiplier = 12f;
    private int _extraJumps;
    [SerializeField] private float _hangTime = .1f;
    [SerializeField] private float _jumpBufferLength = .1f;
    private int _extraJumpsValue;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0f && (_hangTimeCounter > 0f || _extraJumpsValue > 0 || _onWall);
    private bool _isJumping;

    [Header("Dash Variables")] [SerializeField]
    private float _dashSpeed = 15f;

    [SerializeField] private float _dashLength = .3f;
    [SerializeField] private float _dashBufferLength = .1f;
    private float _dashBufferCounter;
    private bool _isDashing;
    private bool _hasDashed;
    private bool _canDash => _dashBufferCounter > 0f && !_hasDashed;

    [Header("Wall Movement Variables")] [SerializeField]
    private float _wallSlideModifier = 0.5f;

    [SerializeField] private float _wallRunModifier = 0.85f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallGrab => _onWall && !_onGround && Input.GetButton("WallGrab") && !_wallRun;

    private bool _wallSlide =>
        _onWall && !_onGround && !Input.GetButton("WallGrab") && _rb.velocity.y < 0f && !_wallRun;

    private bool _wallRun => _onWall && _verticalDirection > 0f;

    #endregion

    #region Unity Funcs

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _extraJumps = Ability.instance.ExtraJumps;
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        if (Input.GetButtonDown("Jump")) _jumpBufferCounter = _jumpBufferLength;
        else _jumpBufferCounter -= Time.deltaTime;
        if (Input.GetButtonDown("Dash")) _dashBufferCounter = _dashBufferLength;
        else _dashBufferCounter -= Time.deltaTime;
        Animation();
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        // Şart sağlandığında alınan input yönünde dash atar
        if (_canDash) StartCoroutine(Dash(_horizontalDirection, _verticalDirection));
        if (!_isDashing)
        {
            if (_canMove)
            {
                MoveCharacter();
            }
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
                _hasDashed = false;
            }
            else
            {
                // Yerde değilse hava sürtünmesini etkinleştirir ve düşüş çarpanını ayarlar
                ApplyAirLinearDrag();
                FallMultiplier();

                // Havada kalma süresini ve zıplama koşulunu kontrol eder
                _hangTimeCounter -= Time.fixedDeltaTime;
                if (!_onWall || _rb.velocity.y < 0f || _wallRun) _isJumping = false;

            }

            if (_canJump)
            {
                // Yerde değilse ve duvardaysa
                if (_onWall && !_onGround)
                {
                    // Duvara tutunduğu halde duvara doğru zıplama gerçekleştiğinde
                    if (!_wallRun && (_onRightWall && _horizontalDirection > 0f ||
                                      !_onRightWall && _horizontalDirection < 0f))
                    {
                        StartCoroutine(NeutralWallJump());
                    }
                    else
                    {
                        WallJump();
                    }

                    Flip();
                }
                // Yerdeyse ve duvara değmiyorsa
                else
                {
                    Jump(Vector2.up);
                }
            }

            if (!_isJumping)
            {
                if (Ability.instance.WallMovement)
                {
                    if (_wallSlide) WallSlide();
                    if (_wallGrab) WallGrab();
                    if (_wallRun) WallRun();
                    if (_onWall) StickToWall();
                }
               
            }
        }

    }

    #endregion

    #region Movement

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

    //Karakterin baktığı yönü tersine çevirir;
    public void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);

       
    }

    #endregion

    #region Drag & Gravity

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

    #endregion

    #region Collisions

    private void CheckCollisions()
    {
        var position = transform.position;

        //Ground Collisions
        _onGround = Physics2D.Raycast(position + _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                        _groundLayer) ||
                    Physics2D.Raycast(position - _groundRaycastOffset, Vector2.down, _groundRaycastLength,
                        _groundLayer);

        //Wall Collisions
        _onWall = Physics2D.Raycast(position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(position, Vector2.right, _wallRaycastLength, _wallLayer);

    }

    #endregion

    #region Jump

    // Zıplama
    private void Jump(Vector2 direction)
    {
        if (!_onGround && !_onWall)
            _extraJumpsValue--;
        ApplyAirLinearDrag();
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
        _hangTimeCounter = 0f;
        _jumpBufferCounter = 0f;
        _isJumping = true;
        EffectManager.Instance.PlayEffectSound(EffectManager.EffectState.JUMP);

    }

    // Duvar üzerindeyken zıplama (Duvarın tersi yönünde zıplandığında çalışacak)
    private void WallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    // Duvarın üzerinde olduğu halde duvara doğru zıplamaya çalıştığında kullanılacak fonksiyon
    IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(_wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
    }

    #endregion

    #region Dash

    //Dash atma 
    IEnumerator Dash(float x, float y)
    {
        if (Ability.instance.CanDash)
        {
            float dashStartTime = Time.time;
            _hasDashed = true;
            _isDashing = true;
            _isJumping = false;

            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0f;
            _rb.drag = 0f;

            // Hareket ediyorsa o yöne doğru dash atar
            Vector2 dir;
            if (x != 0f || y != 0f) dir = new Vector2(x, y);
            else
            {
                //Hareket etmiyorsa yüzü ne tarafa bakıyorsa o yöne dash atar
                if (_facingRight) dir = new Vector2(1f, 0f);
                else dir = new Vector2(-1f, 0f);
            }

            // dashStartTime süresince karakterin hızını input verisi yönünde arttırır
            while (Time.time < dashStartTime + _dashLength)
            {
                _rb.velocity = dir.normalized * _dashSpeed;
                ShakeCamera.ShakeCam.Shake(1f,.1f);

                yield return null;
            }
            EffectManager.Instance.PlayEffectSound(EffectManager.EffectState.DASH);



            _isDashing = false; 
        }
        
          
    }

    #endregion

    #region Wall

    // Duvara tutunma
    void WallGrab()
    {
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
    }

    // Duvardan kayma
    void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMoveSpeed * _wallSlideModifier);
    }

    // Duvarda koşma 
    void WallRun()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _verticalDirection * _maxMoveSpeed * _wallRunModifier);
    }

    // Karakterin yatay hizasını sabit tutar (Duvara yapışık şekilde durmasını sağlar (WallGrab() fonksiyonu duvar üzerinde sabit tutar bu fonksiyon Y hizasına dokunmaz))
    void StickToWall()
    {
        if (_onRightWall && _horizontalDirection >= 0f)
        {
            _rb.velocity = new Vector2(1f, _rb.velocity.y);
        }
        else if (!_onRightWall && _horizontalDirection <= 0f)
        {
            _rb.velocity = new Vector2(-1f, _rb.velocity.y);
        }


        if (_onRightWall && !_facingRight)
        {
            Flip();
        }
        else if (!_onRightWall && _facingRight)
        {
            Flip();
        }
    }

    #endregion

    #region Animation

    void Animation()
    {
        if (_isDashing)
        {
            _anim.SetBool("isDashing", true);
            _anim.SetBool("isGrounded", false);
            _anim.SetBool("isFalling", false);
            _anim.SetBool("WallGrab", false);
            _anim.SetBool("isJumping", false);
            _anim.SetFloat("horizontalDirection", 0f);
            _anim.SetFloat("verticalDirection", 0f);
        }
        else
        {
            _anim.SetBool("isDashing", false);

            if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight) &&
                !_wallGrab && !_wallSlide)
            {
                Flip();
            }

            if (_onGround)
            {
                _anim.SetBool("isGrounded", true);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("horizontalDirection", Mathf.Abs(_horizontalDirection));
            }
            else
            {
                _anim.SetBool("isGrounded", false);
            }

            if (_isJumping)
            {
                _anim.SetBool("isJumping", true);
                _anim.SetBool("isFalling", false);
                _anim.SetBool("WallGrab", false);
                _anim.SetFloat("verticalDirection", 0f);
            }
            else
            {
                _anim.SetBool("isJumping", false);

                if (_wallGrab || _wallSlide)
                {
                    _anim.SetBool("WallGrab", true);
                    _anim.SetBool("isFalling", false);
                    _anim.SetFloat("verticalDirection", 0f);
                }
                else if (_rb.velocity.y < 0f)
                {
                    _anim.SetBool("isFalling", true);
                    _anim.SetBool("WallGrab", false);
                    _anim.SetFloat("verticalDirection", 0f);
                }

                if (_wallRun)
                {
                    _anim.SetBool("isFalling", false);
                    _anim.SetBool("WallGrab", false);
                    _anim.SetFloat("verticalDirection", Mathf.Abs(_verticalDirection));
                }
            }
        }
    }

    #endregion

    #region ParticleEffects

    public void DustParticle()
    {
        velocityModule = _dustParticle.velocityOverLifetime;

        if (_facingRight)
        {
            velocityModule.x = -2.0f;
        }
        else if (!_facingRight)
        {
            velocityModule.x = 2.0f;

        }

        _dustParticle.Play();
    }

    public void BloodParticle()
    {
        _bloodParticle.Play();
    }

    #endregion
}
