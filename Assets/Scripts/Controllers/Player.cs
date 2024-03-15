using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private Rigidbody2D _rigidBody;
}
