using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GoToLobby()
    {
        LevelManager.inst.sessionData.gold += Mathf.RoundToInt(LevelManager.inst.gold);
        SceneManager.LoadSceneAsync(0);
    }
}
