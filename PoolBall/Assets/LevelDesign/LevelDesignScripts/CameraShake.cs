using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake inst { get; private set; }

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }
    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ProcessShake(duration, magnitude));
    }

    private IEnumerator ProcessShake(float duration, float magnitude)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float damper = 1.0f - (elapsedTime / duration);
            Vector3 randomOffset = Random.insideUnitSphere * magnitude * damper;
            randomOffset.z = 0f;
            transform.localPosition = originalLocalPosition + randomOffset;
            yield return null;
        }
        transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }
}