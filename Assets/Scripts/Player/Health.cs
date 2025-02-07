using UnityEngine;
using System;

/// <summary>
/// Reusable health script.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    private AnimationController _anim;
    public bool IsInvincible { get; set; } = false;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _anim = GetComponentInChildren<AnimationController>();
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0) return;
        if (!IsInvincible)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
        }
        _anim.TakeDamage();
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public int GetHealth() => _currentHealth;
    public int GetMaxHealth() => _maxHealth;
}
