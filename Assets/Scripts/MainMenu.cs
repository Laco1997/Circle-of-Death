using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Ovladac umoznujuci prepinanie sa medzi scenarmi v Menu hry.
*/
public class MainMenu : MonoBehaviour { 
    void Start () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /*
     * Spustenie outdoor sveta (hlavna scena).
     */
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    /*
     * Spustenie sceny obsahujuce ovladanie hry.
     */
    public void ShowControls()
    {
        SceneManager.LoadScene("Controls");
    }

    /*
     * Vratenie sa do sceny hlavneho menu.
     */
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /*
     * Zatvorenie hry.
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}
