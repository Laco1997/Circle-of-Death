using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* System pre pocitanie sipov, ktore je prepojene s HUD.
*/
public class ArrowSystem : MonoBehaviour
{
    public int maxArrowCount;
    int currentArrowCount;
    public TMP_Text currentArrowCountText;

    void Start()
    {
        currentArrowCount = maxArrowCount;
    }

    public float getPercentage()
    {
        return (float)currentArrowCount / (float)maxArrowCount * maxArrowCount;

    }

    /*
    * Funkcia ktora pocita pocet vystrelenych sipov.
    */
    public void arrowsUsed(int amount)
    {
        currentArrowCount -= amount;
        if (currentArrowCount <= 0)
        {
            currentArrowCount = 0;
        }

        currentArrowCountText.text = currentArrowCount.ToString();
    }

    /*
    * Funkcia ktora pocita pocet pozbieranych sipov.
    */
    public void arrowsCollected(int amount)
    {
        currentArrowCount += amount;
        if (currentArrowCount >= maxArrowCount)
        {
            currentArrowCount = maxArrowCount;
        }

        currentArrowCountText.text = currentArrowCount.ToString();
    }

    /*
    * Getter pre ziskanie aktualneho poctu sipov.
    */
    public int CurrentArrows
    {
        get { return currentArrowCount; }
    }
}
