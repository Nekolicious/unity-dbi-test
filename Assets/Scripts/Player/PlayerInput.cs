using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages all user inputs and forward them to specified control.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    void Awake()
    {
        if (_player == null)
            _player = GetComponent<PlayerController>();
    }
    public void Move(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            _player.MoveDirection(obj.ReadValue<Vector2>());
        }
        else if (obj.canceled)
        {
            _player.MoveDirection(Vector2.zero);
        }
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            _player.Jump();
        }
        else if (obj.canceled)
        {
            _player.JumpCancel();
        }
    }

    public void Attack(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            _player.Attack();
        }
    }

    public void Down(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            _player.IsDownForce = true;
        }
        else if (obj.canceled)
        {
            _player.IsDownForce = false;
        }
    }

    public void Restart(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SceneMaster.Instance.RestartScene();
        }
    }
}
