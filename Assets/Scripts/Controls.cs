using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Ovladac pre zmenu sceny. Vratenie sa do hlavneho menu z obrazovky Controls.
*/
public class Controls : MonoBehaviour
{
    public void ReturnHome()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
