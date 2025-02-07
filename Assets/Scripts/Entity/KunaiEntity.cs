using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage kunai projectile.
/// </summary>
public class KunaiEntity : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 3f;
    private Rigidbody2D _rb;
    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * speed;

        Invoke(nameof(Deactivate), lifetime);
    }

    private void Deactivate()
    {
        KunaiPool.Instance.ReturnKunai(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out var health))
        {
            // Will ignore the collision if the player is dead
            if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>().IsDead)
            {
                return;
            }
            health.TakeDamage(damage);
        }
        Deactivate();
    }
}
