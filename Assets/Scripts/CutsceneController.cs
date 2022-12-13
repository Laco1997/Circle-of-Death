using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Ovladac pre cutscene medzi levelmi (padanie z lesa do lavovej jaskyni).
*/
public class CutsceneController : MonoBehaviour
{
    float delayTime = 9.4f;

    /*
     * Spustenie casovanej funkcie pre delay po prehrati custscenu.
     */
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delayTime));
    }

    /*
    * Spustenie lavovej sceny po ukonceni cutscenu.
    */
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("LavaScene");
    }
}
