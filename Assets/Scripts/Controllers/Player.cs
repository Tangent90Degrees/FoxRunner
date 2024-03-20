using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region On Inspector
    
    [SerializeField] private float speed;
    [SerializeField] private float crouchSpeedFactor;

    [SerializeField] private float jumpForce;
    [SerializeField] private float keepJumpingForce;
    [SerializeField] private float keepJumpingDuration;

    [Header("Physics Environment Settings"), SerializeField]
    private PhysicsEnvironmentChecker ground;

    [SerializeField] private PhysicsEnvironmentChecker head;
    
    #endregion

    public Vector2 Velocity
    {
        get => _rigidBody.velocity;
        private set => _rigidBody.velocity = value;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _input = new InputSystem();

        var size = _collider.size;
        _colliderState = new ColliderState(size);
        _crouchColliderState = new ColliderState(new Vector2(size.x, size.x));
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Gameplay.Jump.started += Jump;
        _input.Gameplay.Jump.canceled += StopJumping;
    }

    private void OnDisable()
    {
        _input.Gameplay.Jump.canceled -= StopJumping;
        _input.Gameplay.Jump.started -= Jump;
        _input.Disable();
    }

    private void Update()
    {
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
        _isCrouching = ground.Hit && (_moveInput.y < 0 || head.Hit);
        (_isCrouching ? _crouchColliderState : _colliderState).Set(_collider);
        head.Origin = _isCrouching ? _crouchColliderState.Top : _colliderState.Top;
        
        _animator.SetFloat(AnimatorVariable.Horizontal, Mathf.Abs(_moveInput.x));
        _animator.SetFloat(AnimatorVariable.Vertical, _rigidBody.velocity.y);
        _animator.SetBool(AnimatorVariable.Ground, ground.Hit);
        _animator.SetBool(AnimatorVariable.Crouch, _isCrouching);
    }

    private void FixedUpdate()
    {
        var playerTransform = transform;

        var position = playerTransform.position;
        ground.HitRay(position, Vector2.down);
        head.HitRay(position, Vector2.up);
        
        var speedFactor = _isCrouching ? crouchSpeedFactor : 1;
        Velocity = new Vector2(speedFactor * speed * _moveInput.x, Velocity.y);

        playerTransform.localScale = _moveInput.x switch
        {
            > 0 => new Vector3(1, 1, 1),
            < 0 => new Vector3(-1, 1, -1),
            _ => playerTransform.localScale
        };

        KeepJumping();
    }
    
    private void OnDrawGizmos()
    {
        var position = transform.position;
        ground.DrawRay(position, Vector2.down);
        head.DrawRay(position, Vector2.up);
    }

    private void Jump(InputAction.CallbackContext _)
    {
        if (!ground.Hit || _isCrouching) return;
        Velocity = new Vector2(Velocity.x, jumpForce);
        StartCoroutine(JumpForSeconds(keepJumpingDuration));
        return;

        IEnumerator JumpForSeconds(float duration)
        {
            _isJumping = true;
            yield return new WaitForSeconds(duration);
            _isJumping = false;
        }
    }

    private void StopJumping(InputAction.CallbackContext _)
    {
        _isJumping = false;
    }

    private void KeepJumping()
    {
        if (!_isJumping) return;
        _rigidBody.AddForce(keepJumpingForce * Vector2.up);
    }

    private Rigidbody2D _rigidBody;
    private CapsuleCollider2D _collider;
    private Animator _animator;
    private InputSystem _input;

    private Vector2 _moveInput;
    private bool _isJumping;
    private bool _isCrouching;

    private ColliderState _colliderState;
    private ColliderState _crouchColliderState;

    /// <summary>
    /// A physics environment check point.
    /// </summary>
    [Serializable]
    private class PhysicsEnvironmentChecker
    {
        [field: SerializeField] public Vector2 Origin { get; set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public LayerMask Layer { get; private set; }
        
        public RaycastHit2D Hit { get; private set; }

        public RaycastHit2D HitRay(Vector2 position, Vector2 direction)
        {
            Hit = Physics2D.Raycast(position + Origin, direction, Range, Layer);
            return Hit;
        }

        public void DrawRay(Vector2 position, Vector2 direction)
        {
            Gizmos.DrawLine(position + Origin, position + Origin + Range * direction);
        }
    }
    
    private class ColliderState
    {
        public Vector2 Top => _offset + new Vector2(0, 0.5f * _size.y);
        
        public ColliderState(Vector2 size)
        {
            _size = size;
            _offset = new Vector2(0, 0.5f * size.y);
        }

        public void Set(CapsuleCollider2D collider)
        {
            collider.size = _size;
            collider.offset = _offset;
        }
        
        private readonly Vector2 _size;
        private readonly Vector2 _offset;
    }
    
    private static class AnimatorVariable
    {
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Ground = Animator.StringToHash("Ground");
        public static readonly int Crouch = Animator.StringToHash("Crouch");
    }
}