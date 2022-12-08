using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    float delayTime = 9.4f;
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delayTime));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("LavaScene");
    }
}
