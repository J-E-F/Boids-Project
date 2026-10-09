using UnityEngine;
//exit and continu buttons for the pause menu.
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
