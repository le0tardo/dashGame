using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FadeIn : MonoBehaviour
{
    [SerializeField] Image screen;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeInRoutine()
    {
        float alpha = 1.0f;
        Color currentColor = screen.color;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;

            currentColor.a = Mathf.Clamp01(alpha);
            screen.color = currentColor;

            yield return null;
        }

        if (alpha <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
