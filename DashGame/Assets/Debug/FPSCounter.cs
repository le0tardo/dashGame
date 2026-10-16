using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] TextMeshProUGUI fpsText;
    private float accumTime = 0f;
    private int frames = 0;
    private float timeLeft = 0f;

    bool showFps = true;

    private void Awake()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        if (!showFps)
        {
            fpsText.SetText("");
            return;
        }

        float deltaTime = Time.unscaledDeltaTime;
        timeLeft -= deltaTime;
        accumTime += Time.unscaledDeltaTime;
        frames++;

        if (timeLeft <= 0f)
        {
            float fps = frames / accumTime;

            fpsText.SetText("{0:0} FPS", fps);

            if (fps >= 55f)
                fpsText.color = Color.green;
            else if (fps >= 29f)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.red;

            timeLeft = updateInterval;
            accumTime = 0f;
            frames = 0;
        }
    }

    public void ToggleFpsCounter()
    {
        showFps = !showFps;
    }
}
