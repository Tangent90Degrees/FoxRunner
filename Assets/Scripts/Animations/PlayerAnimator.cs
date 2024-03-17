using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _animator.SetFloat(Horizontal, Mathf.Abs(_player.HorizontalMove));
    }

    private Animator _animator;
    private Player _player;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
}
