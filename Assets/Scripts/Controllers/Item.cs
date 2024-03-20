using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Effect collectEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(collectEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
