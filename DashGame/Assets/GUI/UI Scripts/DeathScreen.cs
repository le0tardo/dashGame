using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public void ExitFromDeathScreen()
    {
        Application.Quit();
    }
    public void RestartLevelFromDeathScreen()
    {
        LevelManager.inst.ResetLevel();
    }
}
