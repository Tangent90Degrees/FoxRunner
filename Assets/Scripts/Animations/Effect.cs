using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float duration;
    
    private void OnEnable()
    {
        StartCoroutine(FadeAfterSeconds());
        return;

        IEnumerator FadeAfterSeconds()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
