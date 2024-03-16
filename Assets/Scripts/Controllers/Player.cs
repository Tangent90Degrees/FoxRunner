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
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _environment = GetComponent<PhysicsEnvironment>();
    }

    private void OnEnable()
    {
        _input.JumpPressed += Jump;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(speed * _input.Move.x, _rigidBody.velocity.y);
        
        var playerTransform = transform;
        playerTransform.localScale = _input.Move.x switch
        {
            > 0 => new Vector3(1, 1, 1),
            < 0 => new Vector3(-1, 1, -1),
            _ => playerTransform.localScale
        };
    }
    
    private void Jump(InputAction.CallbackContext _)
    {
        if (!_environment.IsOnGround) return;
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
    }

    private Rigidbody2D _rigidBody;
    private PlayerInput _input;
    private PhysicsEnvironment _environment;
}
