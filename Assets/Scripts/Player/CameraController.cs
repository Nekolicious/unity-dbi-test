using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player camera using deadzone movements method.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _deadzone = new Vector2(2f, 1f);
    [SerializeField] private float _smoothSpeed = 5f;

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = _target.position;

        if (Math.Abs(targetPos.x - camPos.x) > _deadzone.x)
        {
            camPos.x = Mathf.Lerp(camPos.x, targetPos.x, _smoothSpeed * Time.deltaTime);
        }

        if (Math.Abs(targetPos.y - camPos.y) > _deadzone.y)
        {
            camPos.y = Mathf.Lerp(camPos.y, targetPos.y, _smoothSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_deadzone.x * 2, _deadzone.y * 2, 0));
    }
}
