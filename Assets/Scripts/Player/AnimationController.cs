using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manage all animation related stuff including animation event.
/// </summary>
public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sprite;
    private float _effectHitDuration = 0.1f;
    public UnityEvent attackEventStart, attackEventEnd;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void FaceDirection(float direction)
    {
        Vector2 scale = transform.localScale;
        if (direction > 0)
        {
            scale.x = 1;
        }
        else if (direction < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
    }

    public void Attack()
    {
        _animator.SetTrigger("AttackTrigger");
    }

    public void TakeDamage()
    {
        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        Color originalColor = _sprite.color;
        _sprite.color = Color.red;
        yield return new WaitForSeconds(_effectHitDuration);
        _sprite.color = originalColor;
    }

    public void IsRunning(bool state)
    {
        _animator.SetBool("isRunning", state);
    }

    public void IsAirtime(bool state)
    {
        _animator.SetBool("isAirtime", state);
    }

    public void Dead()
    {
        _animator.SetTrigger("DeadTrigger");
    }

    public void EnableDamage()
    {
        attackEventStart?.Invoke();
    }

    public void DisableDamage()
    {
        attackEventEnd?.Invoke();
    }
}
