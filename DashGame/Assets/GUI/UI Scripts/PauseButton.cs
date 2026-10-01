using System.Security.Cryptography;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;


    public void OpenPauseMenu()
    {
        if(pauseMenu!=null && !pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }
}
