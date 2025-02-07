using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Main controller for player, manage all movements and action (not including input).
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private AnimationController _anim;
    private TrailRenderer _tr;
    private Health _health;

    [Header("Movements")]
    [SerializeField] private float _moveSpeed = 16f;
    [SerializeField] private float _jumpForce = 20f;
    [SerializeField] private float _dashForce = 30f;
    [SerializeField] private float _dashCooldown = 3f;
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _baseGravity = 3f;
    [SerializeField] private float _jumpGravity = 1.5f;
    [SerializeField] private float _fallGravity = 4.5f;
    [SerializeField] private float _fastFallGravity = 6f;

    [Header("Ground Sense")]
    [SerializeField] private Transform _feetPos;
    [SerializeField] private LayerMask _groundLayer;
    public Collider2D attackHitBox;
    public bool IsJumping { get; private set; } = false;
    public bool IsRunning { get; private set; } = false;
    public bool IsDashing { get; private set; } = false;
    public bool IsDead { get; private set; } = false;
    public bool IsDownForce { get; set; } = false;
    public bool IsAirtime { get; private set; } = false;
    public bool CanDashAttack { get; private set; } = true;
    private float _moveDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
        _anim = GetComponentInChildren<AnimationController>();
        _health = GetComponent<Health>();

        _health.OnDeath += HandleDeath;

        _anim.attackEventStart.AddListener(EnableHitBox);
        _anim.attackEventEnd.AddListener(DisableHitBox);
    }

    void FixedUpdate()
    {
        if (IsDead) return;
        GravityHandler();

        if (!IsGrounded())
        {
            IsAirtime = true;
            _anim.IsAirtime(IsAirtime);
        }
        else
        {
            IsAirtime = false;
            _anim.IsAirtime(IsAirtime);
        }

        if (IsDashing) return;
        Move();
    }

    public void MoveDirection(Vector2 direction)
    {
        _moveDirection = direction.x;
        if (direction.x > 0 || direction.x < 0)
        {
            Vector2 scale = attackHitBox.transform.localScale;
            scale.x = direction.x;
            attackHitBox.transform.localScale = scale;
        }
    }

    private void Move()
    {
        _anim.FaceDirection(_moveDirection);
        _rb.velocity = new Vector2(_moveDirection * _moveSpeed, _rb.velocity.y);
        IsRunning = true;
        _anim.IsRunning(IsRunning);
        if (_rb.velocity == Vector2.zero)
        {
            IsRunning = false;
            _anim.IsRunning(IsRunning);
        }
    }

    private void GravityHandler()
    {
        if (_rb.velocity.y > 0)
        {
            _rb.gravityScale = _jumpGravity;
            DownForceHandler();
        }
        else if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _fallGravity;
            DownForceHandler();
        }
        else
        {
            _rb.gravityScale = _baseGravity;
        }
    }

    private void DownForceHandler()
    {
        if (IsDownForce && !IsGrounded())
        {
            _rb.gravityScale = _fastFallGravity;
        }
    }

    public void Jump()
    {
        if (IsDead) return;
        if (IsGrounded() && !IsJumping)
        {
            IsJumping = true;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void JumpCancel()
    {
        IsJumping = false;
        if (_rb.velocity.y > 0)
        {
            if (IsDownForce) return;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
    }

    public void Attack()
    {
        // If player is running use dash attack instead of normal attack
        if (IsRunning && !IsAirtime)
        {
            if (!CanDashAttack) return;
            StartCoroutine(DashAttack());
        }
        _anim.Attack();
    }

    IEnumerator DashAttack()
    {
        CanDashAttack = false;
        IsDashing = true;
        _anim.Attack();
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _tr.emitting = true;
        _rb.velocity = new Vector2(_anim.transform.localScale.x * _dashForce, 0f);
        yield return new WaitForSeconds(_dashTime);
        IsDashing = false;
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(_dashCooldown);
        CanDashAttack = true;
    }

    private void HandleDeath()
    {
        IsDead = true;
        _anim.Dead();
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(_feetPos.position, new Vector2(2.2f, 0.2f), 0f, _groundLayer);
    }

    private void EnableHitBox()
    {
        attackHitBox.enabled = true;
    }

    private void DisableHitBox()
    {
        attackHitBox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(10);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetPos.position, new Vector3(2.2f, 0.2f, 0));
    }
}
