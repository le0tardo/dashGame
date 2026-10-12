using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    TextMeshProUGUI fpsTargetText;
    int targetFps = 30;
    private void Start()
    {
        fpsTargetText=GetComponentInChildren<TextMeshProUGUI>();
        SetTargetFPS(targetFps);
    }
    void SetTargetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        print("set target fps to: " + fps);
        fpsTargetText.text = "Target FPS: " + fps;
    }

    public void ToggleTargetFPS()
    {
        if (targetFps == 30)
        {
            targetFps = 60;
        }
        else
        {
            targetFps = 30;
        }
        SetTargetFPS(targetFps);
    }
}
