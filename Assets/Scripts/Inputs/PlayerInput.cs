using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input managing component for players.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public event Action<InputAction.CallbackContext> JumpPressed
    {
        add => _input.Gameplay.Fire.started += value;
        remove => _input.Gameplay.Fire.started -= value;
    }
    
    /// <summary>
    /// The vector2 value read from player move input.
    /// </summary>
    public Vector2 Move => _input.Gameplay.Move.ReadValue<Vector2>();

    private void Awake()
    {
        _input = new InputSystem();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private InputSystem _input;
}
