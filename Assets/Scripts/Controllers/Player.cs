using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(PhysicsEnvironment))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// The walking speed of player.
    /// </summary>
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float keepJumpingForce;
    [SerializeField] private float keepJumpingDuration;

    public float HorizontalMove => _input.Move.x;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _environment = GetComponent<PhysicsEnvironment>();
    }

    private void OnEnable()
    {
        _input.Jump.Pressed += Jump;
        _input.Jump.Released += StopJumping;
    }

    private void OnDisable()
    {
        _input.Jump.Pressed -= Jump;
        _input.Jump.Released -= StopJumping;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(speed * HorizontalMove, _rigidBody.velocity.y);
        
        var playerTransform = transform;
        playerTransform.localScale = HorizontalMove switch
        {
            > 0 => new Vector3(1, 1, 1),
            < 0 => new Vector3(-1, 1, -1),
            _ => playerTransform.localScale
        };

        KeepJumping();
    }
    
    private void Jump(InputAction.CallbackContext _)
    {
        if (!_environment.IsOnGround) return;
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
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
    private PlayerInput _input;
    private PhysicsEnvironment _environment;

    private bool _isJumping;
}
