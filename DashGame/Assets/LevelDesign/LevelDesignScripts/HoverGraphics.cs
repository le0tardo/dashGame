using UnityEngine;

public class HoverGraphics : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float heightOffset = 0.1f;
    [SerializeField] private float cycleDuration = 2.5f;
    [SerializeField] private bool randomizeOffset = true;

    private Vector3 startLocalPos;
    private float timeOffset;

    private void Awake()
    {
        startLocalPos = transform.localPosition;

        if (randomizeOffset)
        {
            timeOffset = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    private void OnEnable()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float radians = ((Time.time / cycleDuration) * Mathf.PI * 2f) + timeOffset;

        float yOffset = Mathf.Sin(radians) * heightOffset;

        transform.localPosition = startLocalPos + new Vector3(0f, yOffset, 0f);
    }
}
