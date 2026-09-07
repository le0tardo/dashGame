using UnityEngine;
using System.Collections;
public class SlidingDoorBehaviour : MonoBehaviour, IButtonAction
{
    [SerializeField] Vector3 closedPosition;
    [SerializeField] Vector3 openPosition;
    [SerializeField] float slideDuration=0.25f;

    private void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = new Vector3(transform.localPosition.x - 6, transform.localPosition.y, transform.localPosition.z);
    }
    public void ButtonAction(bool pressed)
    {
        if (pressed)
        {
            StopAllCoroutines();
            StartCoroutine(SlideRoutine(openPosition));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SlideRoutine(closedPosition));
        }
    }

    IEnumerator SlideRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.localPosition;

        float totalDistance = Vector3.Distance(closedPosition, openPosition);
        float remainingDistance = Vector3.Distance(startPosition, targetPosition);

        if (totalDistance <= 0.001f)
        {
            transform.localPosition = targetPosition;
            yield break;
        }

        float dynamicDuration = slideDuration * (remainingDistance / totalDistance);

        float elapsedTime = 0f;

        while (elapsedTime < dynamicDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / dynamicDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.localPosition = targetPosition;
    }
}
