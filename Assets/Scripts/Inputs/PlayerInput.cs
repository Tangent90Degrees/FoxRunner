using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input managing component for players.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public ButtonInput Jump;
    
    /// <summary>
    /// The vector2 value read from player move input.
    /// </summary>
    public Vector2 Move => _input.Gameplay.Move.ReadValue<Vector2>();

    private void Awake()
    {
        _input = new InputSystem();
        Jump = new ButtonInput(_input.Gameplay.Jump);
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

    public class ButtonInput
    {
        public ButtonInput(InputAction input)
        {
            _input = input;
        }
        
        public event Action<InputAction.CallbackContext> Pressed
        {
            add => _input.started += value;
            remove => _input.started -= value;
        }
        
        public event Action<InputAction.CallbackContext> Released
        {
            add => _input.canceled += value;
            remove => _input.canceled -= value;
        }
        
        private readonly InputAction _input;
    }
}
