using System;
using UnityEngine;

public class PhysicsEnvironment : MonoBehaviour
{
    [SerializeField] private Checker ground;

    public bool IsOnGround => ground.Hit(transform.position, Vector2.down);

    private void OnDrawGizmos()
    {
        ground.DrawRay(transform.position, Vector2.down);
    }

    [Serializable]
    private class Checker
    {
        [field: SerializeField] public Vector2 Origin { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public LayerMask Layer { get; private set; }

        public RaycastHit2D Hit(Vector2 position, Vector2 direction)
        {
            return Physics2D.Raycast(position + Origin, direction, Range, Layer);
        }
        
        public void DrawRay(Vector2 position, Vector2 direction)
        {
            Gizmos.DrawLine(position + Origin, position + Origin + Range * direction);
        }
    }
}
