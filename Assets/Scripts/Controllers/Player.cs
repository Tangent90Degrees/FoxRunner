using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// The walking speed of player.
    /// </summary>
    [SerializeField] private float speed;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
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

    private Rigidbody2D _rigidBody;
    private PlayerInput _input;
}
