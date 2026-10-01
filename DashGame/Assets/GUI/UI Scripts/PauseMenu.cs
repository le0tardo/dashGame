using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private void OnEnable()
    {
        print("Pause menu enabled");
        LevelManager.inst.timeScale = 0;
    }

    private void OnDisable()
    {
        print("Pause menu disabled");
        LevelManager.inst.timeScale = 1;
    }

    public void ResumeButton()
    {
        this.gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        LevelManager.inst.ResetLevel();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
