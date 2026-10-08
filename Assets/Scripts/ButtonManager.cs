using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    public void continueGame()
    {
        pauseMenu.Resume();
    }
}
