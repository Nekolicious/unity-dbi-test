using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Master code for dummy.
/// </summary>
public class DummyEntity : MonoBehaviour
{
    private PlayerController _dummy;
    private Health _health;

    void Awake()
    {
        _dummy = GetComponent<PlayerController>();
        _health = GetComponent<Health>();

        // Dummy still damagable but won't reduce its health.
        _health.IsInvincible = true;
    }
}
