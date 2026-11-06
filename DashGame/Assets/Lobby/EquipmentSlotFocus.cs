using System.Collections;
using UnityEngine;

public class EquipmentSlotFocus : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RectTransform helmets;
    [SerializeField] private RectTransform mainHands;
    [SerializeField] private RectTransform offHands;
    [SerializeField] private RectTransform pets;

    [Header("Positions & Settings")]
    [SerializeField] private float showPositionX = -1080f;
    [SerializeField] private float hidePositionX = -1616;
    [SerializeField] private float duration = 0.3f;

    private RectTransform[] allPanels;
    private Coroutine slideRoutine;

    private void Awake()
    {
        // Cache all panels into an array for easy iteration
        allPanels = new RectTransform[] { helmets, mainHands, offHands, pets };
    }

    public void ShowHelmets() => SelectPanel(helmets);
    public void ShowMainHands() => SelectPanel(mainHands);
    public void ShowOffHands() => SelectPanel(offHands);
    public void ShowPets() => SelectPanel(pets);

    private void SelectPanel(RectTransform targetPanel)
    {
        // If a slide animation is running, stop it to start the new transition smoothly
        if (slideRoutine != null)
        {
            StopCoroutine(slideRoutine);
        }
        slideRoutine = StartCoroutine(SlideObjects(targetPanel));
    }

    private IEnumerator SlideObjects(RectTransform objectToShow)
    {
        float elapsedTime = 0f;

        // Cache starting positions for all panels to ensure zero allocations during Lerp
        Vector2[] startPositions = new Vector2[allPanels.Length];
        Vector2[] targetPositions = new Vector2[allPanels.Length];

        for (int i = 0; i < allPanels.Length; i++)
        {
            if (allPanels[i] == null) continue;

            startPositions[i] = allPanels[i].anchoredPosition;

            // Determine target position based on whether this panel is selected
            float targetX = (allPanels[i] == objectToShow) ? showPositionX : hidePositionX;
            targetPositions[i] = new Vector2(targetX, startPositions[i].y);
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Apply smooth easing step
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            for (int i = 0; i < allPanels.Length; i++)
            {
                if (allPanels[i] == null) continue;
                allPanels[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], smoothT);
            }

            yield return null;
        }

        // Lock final target positions to eliminate minor floating-point drift
        for (int i = 0; i < allPanels.Length; i++)
        {
            if (allPanels[i] == null) continue;
            allPanels[i].anchoredPosition = targetPositions[i];
        }

        slideRoutine = null;
    }
}